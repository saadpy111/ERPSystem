using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    /// <summary>
    /// Reverses a posted journal entry by creating a new, independent reversal journal.
    ///
    /// Immutable ledger rule:
    ///   The original journal entry is NEVER modified, updated, or soft-deleted.
    ///   It remains in the ledger exactly as posted — this is the foundation of
    ///   audit trail integrity in any enterprise accounting system.
    ///
    /// Unidirectional link rule:
    ///   Only the REVERSAL journal holds a reference to the original (ReversedSourceJournalId).
    ///   The original journal is never written back to. This eliminates circular dependencies
    ///   and synchronization bugs between the two entries.
    ///
    /// The reversal entry swaps Debit ↔ Credit on every line of the original, creating
    /// an equal-and-opposite accounting entry that nets the original to zero in the ledger.
    /// </summary>
    public class ReverseJournalEntryCommandHandler : IRequestHandler<ReverseJournalEntryCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<FiscalPeriod> _fiscalPeriodRepository;

        public ReverseJournalEntryCommandHandler(
            IUnitOfWork uow,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository)
        {
            _uow = uow;
            _fiscalPeriodRepository = fiscalPeriodRepository;
        }

        public async Task<Result<int>> Handle(
            ReverseJournalEntryCommand request,
            CancellationToken cancellationToken)
        {
            return await _uow.ExecuteTransactionAsync(async () =>
            {
                // ── Load original entry ───────────────────────────────────────────────
                var original = await _uow.JournalEntries.GetByIdWithLinesAsync(request.JournalEntryId);

                if (original == null)
                    return Result<int>.Failure(
                        $"Journal entry with ID {request.JournalEntryId} not found.");

                if (original.Status != JournalStatus.Posted)
                    return Result<int>.Failure(
                        "Only posted journal entries can be reversed.");

                if (original.Lines == null || !original.Lines.Any())
                    return Result<int>.Failure(
                        "Original journal entry has no lines to reverse.");

                // ── Prevent double reversal ───────────────────────────────────────────
                // Check whether a reversal journal already references this entry as its source.
                // The original is NEVER mutated so we query the reversal side.
                var existingReversals = await _uow.JournalEntries
                    .FindAsync(e => e.ReversedSourceJournalId == original.Id);

                if (existingReversals.Any())
                    return Result<int>.Failure(
                        "Journal entry has already been reversed. Double reversal is not permitted.");

                if (original.CurrencyId <= 0)
                    return Result<int>.Failure("Invalid currency on original entry.");

                // ── Validate reversal date falls in an open fiscal period ─────────────
                var openPeriods = await _fiscalPeriodRepository
                    .FindAsync(p =>
                        !p.IsClosed &&
                        request.ReversalDate >= p.StartDate &&
                        request.ReversalDate <= p.EndDate);

                var targetPeriod = openPeriods.FirstOrDefault();

                if (targetPeriod == null)
                    return Result<int>.Failure(
                        "The specified reversal date does not fall into an open fiscal period.");

                // ── Generate reversed lines (swap Debit ↔ Credit) ─────────────────────
                var newLines = new List<JournalEntryLine>();

                foreach (var line in original.Lines)
                {
                    newLines.Add(new JournalEntryLine
                    {
                        AccountId     = line.AccountId,
                        Debit         = line.Credit,         // swap
                        Credit        = line.Debit,          // swap
                        PartnerId     = line.PartnerId,
                        CostCenterId  = line.CostCenterId,
                        CurrencyId    = line.CurrencyId,
                        ForeignAmount = line.ForeignAmount,  // carry forward from original
                        ExchangeRate  = line.ExchangeRate,   // historical rate — never changed
                        BaseAmount    = line.BaseAmount,     // carry forward (positive — sign tracked by Dr/Cr)
                        Description   = string.IsNullOrWhiteSpace(line.Description)
                                            ? "Reversal"
                                            : $"Reversal – {line.Description}"
                    });
                }

                if (newLines.Count < 2)
                    return Result<int>.Failure(
                        "Reversal failed: generated lines are less than 2.");

                var totalDebit  = Math.Round(newLines.Sum(l => l.Debit),  6);
                var totalCredit = Math.Round(newLines.Sum(l => l.Credit), 6);

                if (totalDebit != totalCredit)
                    return Result<int>.Failure(
                        $"Reversal failed: unbalanced entry. Debit: {totalDebit}, Credit: {totalCredit}.");

                // ── Build the reversal journal entry ──────────────────────────────────
                // The reversal journal is a completely independent posted entry.
                // It holds ReversedSourceJournalId → the only link between the two entries.
                // The original journal is NEVER touched.
                var reversalEntry = new JournalEntry
                {
                    JournalNumber          = "REV-JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Date                   = request.ReversalDate,
                    Reference              = original.Reference,
                    CurrencyId             = original.CurrencyId,
                    SourceType             = SourceType.Reversal,
                    SourceId               = original.Id,
                    Description            = $"Reversal of JE #{original.JournalNumber}",
                    Status                 = JournalStatus.Posted,
                    TotalDebit             = totalDebit,
                    TotalCredit            = totalCredit,
                    PostedAt               = DateTime.UtcNow,
                    PostedBy               = request.ReversedBy,
                    FiscalPeriodId         = targetPeriod.Id,
                    ReversedSourceJournalId = original.Id,   // unidirectional link only
                    ReversedAt             = DateTime.UtcNow,
                    ReversedBy             = request.ReversedBy,
                    ReversalReason         = request.Reason ?? "Manual reversal",
                    Lines                  = newLines
                };

                await _uow.JournalEntries.AddAsync(reversalEntry);

                // ── Single atomic save ────────────────────────────────────────────────
                // Only the NEW reversal entry is written.
                // The original journal entry remains completely unchanged in the ledger.
                await _uow.SaveChangesAsync();

                return Result<int>.Ok(reversalEntry.Id, "Journal entry successfully reversed.");

            }, cancellationToken);
        }
    }
}
