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
    public class PayablePaymentPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IUnitOfWork _uow;

        public PayablePaymentPostingStrategy(IAccountingMappingService mappingService, IUnitOfWork uow)
        {
            _mappingService = mappingService;
            _uow = uow;
        }

        public bool CanHandle(SourceType type) => type == SourceType.PayablePayment;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var payment = await _uow.PayablePayments.GetByIdAsync(request.SourceId);

            if (payment == null)
                return Result<List<JournalEntryLine>>.Failure($"Payable Payment with ID {request.SourceId} not found.");

            if (payment.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure("Invalid payable payment amount.");

            var cashAccount = await _uow.CashAccounts.GetByIdAsync(payment.CashAccountId);
            if (cashAccount == null || cashAccount.AccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure($"Invalid Cash Account setup for payment ID {request.SourceId}.");

            var apAccountId = await _mappingService.GetAccountIdAsync(SourceType.PayablePayment, MappingKeys.AccountsPayable);

            if (apAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Accounts Payable.");

            var payable = await _uow.Payables.GetByIdAsync(payment.PayableId);
            int? partnerId = payable?.PartnerId;

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine
                {
                    AccountId = apAccountId,
                    Debit = payment.Amount,
                    Credit = 0,
                    PartnerId = partnerId,
                    Description = payment.Description ?? "Accounts Payable Payment"
                },
                new JournalEntryLine
                {
                    AccountId = cashAccount.AccountId,
                    Debit = 0,
                    Credit = payment.Amount,
                    PartnerId = partnerId,
                    Description = payment.Description ?? "Cash Payment for Payable"
                }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
