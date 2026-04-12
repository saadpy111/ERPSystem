using Accounting.Application.Common.Constants;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    public class ReceivablePaymentPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IUnitOfWork _uow;

        public ReceivablePaymentPostingStrategy(IAccountingMappingService mappingService, IUnitOfWork uow)
        {
            _mappingService = mappingService;
            _uow = uow;
        }

        public bool CanHandle(SourceType type) => type == SourceType.ReceivablePayment;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var payment = await _uow.ReceivablePayments.GetByIdAsync(request.SourceId);

            if (payment == null)
                return Result<List<JournalEntryLine>>.Failure($"Receivable Payment with ID {request.SourceId} not found.");

            if (payment.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure("Invalid receivable payment amount.");

            var cashAccount = await _uow.CashAccounts.GetByIdAsync(payment.CashAccountId);
            if (cashAccount == null || cashAccount.AccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure($"Invalid Cash Account setup for payment ID {request.SourceId}.");

            var arAccountId = await _mappingService.GetAccountIdAsync(SourceType.ReceivablePayment, MappingKeys.AccountsReceivable);

            if (arAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Accounts Receivable.");

            // To get partner id, we fetch the receivable
            var receivable = await _uow.Receivables.GetByIdAsync(payment.ReceivableId);
            int? partnerId = receivable?.PartnerId;

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine
                {
                    AccountId = cashAccount.AccountId,
                    Debit = payment.Amount,
                    Credit = 0,
                    PartnerId = partnerId,
                    Description = payment.Description ?? "Cash Receipt for Receivable"
                },
                new JournalEntryLine
                {
                    AccountId = arAccountId,
                    Debit = 0,
                    Credit = payment.Amount,
                    PartnerId = partnerId,
                    Description = payment.Description ?? "Accounts Receivable Payment"
                }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
