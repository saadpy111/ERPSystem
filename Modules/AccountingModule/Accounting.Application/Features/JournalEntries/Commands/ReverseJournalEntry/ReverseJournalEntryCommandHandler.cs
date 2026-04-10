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
    public class ReverseJournalEntryCommandHandler : IRequestHandler<ReverseJournalEntryCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReverseJournalEntryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(ReverseJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var original = await _unitOfWork.JournalEntries.GetByIdWithLinesAsync(request.JournalEntryId);

            if (original == null)
                return Result<int>.Failure($"Journal Entry with ID {request.JournalEntryId} was not found.");

            if (original.Status != JournalStatus.Posted)
                return Result<int>.Failure("Only Posted journal entries can be reversed.");

            if (original.IsReversed)
                return Result<int>.Failure("This journal entry has already been reversed.");

            if (!original.Lines.Any())
                return Result<int>.Failure("The journal entry has no lines and cannot be reversed.");

            if (original.FiscalPeriod == null)
                return Result<int>.Failure("Fiscal period information could not be loaded for this entry.");

            if (original.FiscalPeriod.IsClosed)
                return Result<int>.Failure($"Cannot reverse: fiscal period '{original.FiscalPeriod.PeriodName}' is closed.");

            return await _unitOfWork.ExecuteTransactionAsync<Result<int>>(async () =>
            {
                var now = DateTime.UtcNow;

                var reversal = new JournalEntry
                {
                    JournalNumber  = $"REV-{original.JournalNumber}",
                    Date           = now,
                    Reference      = string.IsNullOrWhiteSpace(original.Reference)
                                         ? $"REV-{original.JournalNumber}"
                                         : $"REV-{original.Reference}",
                    CurrencyId     = original.CurrencyId,
                    SourceType     = SourceType.Reversal,
                    Description    = $"Reversal of {original.JournalNumber} - {request.Reason}",
                    Status         = JournalStatus.Posted,
                    PostedAt       = now,
                    TotalDebit     = original.TotalCredit,
                    TotalCredit    = original.TotalDebit,
                    FiscalPeriodId = original.FiscalPeriodId,
                    TenantId       = original.TenantId,
                    CreatedAt      = now,
                    CreatedBy      = "System",
                    ReversedEntryId = original.Id,
                    ReversalReason  = request.Reason,
                };

                foreach (var line in original.Lines)
                {
                    reversal.Lines.Add(new JournalEntryLine
                    {
                        AccountId     = line.AccountId,
                        Debit         = line.Credit,
                        Credit        = line.Debit,
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

                original.IsReversed     = true;
                original.ReversedAt     = now;
                original.ReversalReason = request.Reason;
                original.Status         = JournalStatus.Reversed;

                _unitOfWork.JournalEntries.Update(original);
                await _unitOfWork.JournalEntries.AddAsync(reversal);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Ok(reversal.Id, "Journal entry reversed successfully.");
            }, cancellationToken);
        }
    }
}
