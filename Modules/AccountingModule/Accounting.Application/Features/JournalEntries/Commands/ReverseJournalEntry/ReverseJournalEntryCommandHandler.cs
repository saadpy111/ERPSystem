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

        public async Task<Result<int>> Handle(ReverseJournalEntryCommand request, CancellationToken cancellationToken)
        {
            return await _uow.ExecuteTransactionAsync(async () =>
            {
                var original = await _uow.JournalEntries.GetByIdWithLinesAsync(request.JournalEntryId);

                if (original == null)
                    return Result<int>.Failure($"Journal entry with ID {request.JournalEntryId} not found.");

                if (original.Status != JournalStatus.Posted)
                    return Result<int>.Failure("Only posted journal entries can be reversed.");

                // Prevent race condition (inside transaction)
                if (original.IsReversed)
                    return Result<int>.Failure("Journal entry already reversed.");

                if (original.CurrencyId <= 0)
                    return Result<int>.Failure("Invalid currency on original entry.");

                if (original.Lines == null || !original.Lines.Any())
                    return Result<int>.Failure("Original journal entry has no lines.");

                var openPeriods = await _fiscalPeriodRepository
                    .FindAsync(p => !p.IsClosed &&
                                    request.ReversalDate >= p.StartDate &&
                                    request.ReversalDate <= p.EndDate);

                var targetPeriod = openPeriods.FirstOrDefault();

                if (targetPeriod == null)
                    return Result<int>.Failure("The specified reversal date does not fall into an open fiscal period.");

                // Generate reversed lines
                var newLines = new List<JournalEntryLine>();

                foreach (var line in original.Lines)
                {
                    newLines.Add(new JournalEntryLine
                    {
                        AccountId = line.AccountId,
                        Debit = line.Credit,   // swap
                        Credit = line.Debit,   // swap
                        PartnerId = line.PartnerId,
                        CostCenterId = line.CostCenterId,
                        CurrencyId = line.CurrencyId,
                        ForeignAmount = line.ForeignAmount,
                        ExchangeRate = line.ExchangeRate,
                        BaseAmount = line.BaseAmount, // safe if system uses positive values
                        Description = string.IsNullOrWhiteSpace(line.Description)
                            ? "Reversal"
                            : $"Reversal - {line.Description}"
                    });
                }

                if (newLines.Count < 2)
                    return Result<int>.Failure("Reversal failed: generated lines are less than 2.");

                var totalDebit = Math.Round(newLines.Sum(l => l.Debit), 6);
                var totalCredit = Math.Round(newLines.Sum(l => l.Credit), 6);

                if (totalDebit != totalCredit)
                    return Result<int>.Failure($"Reversal failed: unbalanced entry. Debit: {totalDebit}, Credit: {totalCredit}.");

                var reversalEntry = new JournalEntry
                {
                    JournalNumber = "REV-JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Date = request.ReversalDate,
                    Reference = original.Reference,
                    CurrencyId = original.CurrencyId,

                    // Source tracking
                    SourceType = SourceType.Reversal,
                    SourceId = original.Id,

                    Description = $"Reversal of JE #{original.JournalNumber}",
                    Status = JournalStatus.Posted,
                    TotalDebit = totalDebit,
                    TotalCredit = totalCredit,
                    PostedAt = DateTime.UtcNow,
                    FiscalPeriodId = targetPeriod.Id,

                    // link to original
                    ReversedEntryId = original.Id,

                    Lines = newLines
                };

                await _uow.JournalEntries.AddAsync(reversalEntry);

                // Update original
                original.IsReversed = true;
                original.ReversalEntryId = reversalEntry.Id;
                original.ReversedAt = DateTime.UtcNow;
                original.ReversalReason = request.Reason ?? "Manual reversal";

                _uow.JournalEntries.Update(original);

                // Single save (atomic)
                await _uow.SaveChangesAsync();

                return Result<int>.Ok(reversalEntry.Id, "Journal entry successfully reversed.");
            }, cancellationToken);
        }
    }
}
