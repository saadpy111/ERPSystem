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
    public class PayablePostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IUnitOfWork _uow;

        public PayablePostingStrategy(IAccountingMappingService mappingService, IUnitOfWork uow)
        {
            _mappingService = mappingService;
            _uow = uow;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Payable;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var payable = await _uow.Payables.GetByIdAsync(request.SourceId);

            if (payable == null)
                return Result<List<JournalEntryLine>>.Failure($"Payable with ID {request.SourceId} not found.");

            if (payable.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure("Invalid payable amount.");

            var expenseAccountId = await _mappingService.GetAccountIdAsync(SourceType.Payable, MappingKeys.PurchaseDebit);
            var apAccountId = await _mappingService.GetAccountIdAsync(SourceType.Payable, MappingKeys.AccountsPayable);

            if (expenseAccountId <= 0 || apAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Payable posting.");

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine
                {
                    AccountId = expenseAccountId,
                    Debit = payable.Amount,
                    Credit = 0,
                    PartnerId = payable.PartnerId,
                    Description = payable.Description ?? "Purchase / Expense"
                },
                new JournalEntryLine
                {
                    AccountId = apAccountId,
                    Debit = 0,
                    Credit = payable.Amount,
                    PartnerId = payable.PartnerId,
                    Description = payable.Description ?? "Account Payable"
                }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
