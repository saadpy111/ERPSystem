using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    /// <summary>
    /// Handles the full reversal lifecycle of a Posted journal entry:
    ///   1. Validates all business rules (status, already-reversed, fiscal-period open).
    ///   2. Creates a mirror entry with every line's Debit ↔ Credit swapped.
    ///   3. Marks the original as Reversed.
    ///   4. Persists everything in a single atomic transaction.
    /// No data is deleted — financial integrity and auditability are preserved.
    /// </summary>
    public class ReverseJournalEntryCommandHandler : IRequestHandler<ReverseJournalEntryCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReverseJournalEntryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(ReverseJournalEntryCommand request, CancellationToken cancellationToken)
        {
            // ── A. Load original entry with Lines + FiscalPeriod ──────────────────
            //       GetByIdWithLinesAsync eagerly includes both navigations so we can
            //       validate fiscal-period status and clone lines without extra queries.
            var original = await _unitOfWork.JournalEntries
                .GetByIdWithLinesAsync(request.JournalEntryId);

            // ── B. Business-rule validation (all done before the transaction) ──────

            if (original == null)
                throw new BusinessException(
                    $"Journal Entry with ID {request.JournalEntryId} was not found.");

            if (original.Status != JournalStatus.Posted)
                throw new BusinessException(
                    "Only Posted journal entries can be reversed.");

            if (original.IsReversed)
                throw new BusinessException(
                    "This journal entry has already been reversed.");

            if (!original.Lines.Any())
                throw new BusinessException(
                    "The journal entry has no lines and cannot be reversed.");

            // B4. Fiscal Period must be OPEN ─────────────────────────────────────
            if (original.FiscalPeriod == null)
                throw new BusinessException(
                    "Fiscal period information could not be loaded for this entry.");

            if (original.FiscalPeriod.IsClosed)
                throw new BusinessException(
                    $"Cannot reverse: fiscal period '{original.FiscalPeriod.PeriodName}' is closed. " +
                    "Please reopen the period or post the reversal to an open period.");

            // ── C–F. Execute in a single database transaction ─────────────────────
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                var now = DateTime.UtcNow;

                // ── C. Build the reversal journal entry ───────────────────────────
                var reversal = new JournalEntry
                {
                    // Journal number: "REV-<original>" — unique because original can
                    // only be reversed once (IsReversed guard above).
                    JournalNumber  = $"REV-{original.JournalNumber}",
                    Date           = now,

                    // Reference mirrors the original, prefixed with REV-
                    Reference      = string.IsNullOrWhiteSpace(original.Reference)
                                         ? $"REV-{original.JournalNumber}"
                                         : $"REV-{original.Reference}",

                    CurrencyId     = original.CurrencyId,
                    SourceType     = SourceType.Reversal,
                    Description    = $"Reversal of {original.JournalNumber} - {request.Reason}",

                    // Auto-posted: a reversal is immediately effective.
                    Status         = JournalStatus.Posted,
                    PostedAt       = now,

                    // Totals are swapped (debit becomes credit, credit becomes debit)
                    TotalDebit     = original.TotalCredit,
                    TotalCredit    = original.TotalDebit,

                    // Stays in the same fiscal period as the original entry.
                    FiscalPeriodId = original.FiscalPeriodId,

                    TenantId       = original.TenantId,
                    CreatedAt      = now,
                    CreatedBy      = "System",   // TODO: replace with ICurrentUserService.UserId

                    // Audit link: ties this reversal back to the entry it negates.
                    ReversedEntryId = original.Id,
                    ReversalReason  = request.Reason,
                };

                // ── D. Reverse every line (Debit ↔ Credit) ────────────────────────
                //       AccountId, PartnerId, CostCenterId, currency info are kept
                //       unchanged to preserve the full audit trail of the original.
                foreach (var line in original.Lines)
                {
                    reversal.Lines.Add(new JournalEntryLine
                    {
                        AccountId     = line.AccountId,
                        Debit         = line.Credit,      // ← swapped
                        Credit        = line.Debit,       // ← swapped
                        Description   = line.Description,
                        PartnerId     = line.PartnerId,
                        CostCenterId  = line.CostCenterId,
                        CurrencyId    = line.CurrencyId,
                        ForeignAmount = line.ForeignAmount,
                        ExchangeRate  = line.ExchangeRate,
                        BaseAmount    = line.BaseAmount,
                        TenantId      = line.TenantId,
                    });
                }

                // ── E. Stamp the original as Reversed ─────────────────────────────
                //       The original entry rows are never deleted; only these three
                //       tracking fields change so the ledger stays fully auditable.
                original.IsReversed     = true;
                original.ReversedAt     = now;
                original.ReversalReason = request.Reason;
                original.Status         = JournalStatus.Reversed;
                // original.ReversedBy  = _currentUserService.UserId;  // TODO

                _unitOfWork.JournalEntries.Update(original);

                // ── F. Persist both in one transaction ────────────────────────────
                await _unitOfWork.JournalEntries.AddAsync(reversal);
                await _unitOfWork.SaveChangesAsync();

                // Return the NEW reversal entry's ID to the caller.
                return reversal.Id;

            }, cancellationToken);
        }
    }
}
