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
    public class ReceivablePostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IUnitOfWork _uow;

        public ReceivablePostingStrategy(IAccountingMappingService mappingService, IUnitOfWork uow)
        {
            _mappingService = mappingService;
            _uow = uow;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Receivable;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var receivable = await _uow.Receivables.GetByIdAsync(request.SourceId);

            if (receivable == null)
                return Result<List<JournalEntryLine>>.Failure($"Receivable with ID {request.SourceId} not found.");

            if (receivable.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure("Invalid receivable amount.");

            var arAccountId = await _mappingService.GetAccountIdAsync(SourceType.Receivable, MappingKeys.AccountsReceivable);
            var revenueAccountId = await _mappingService.GetAccountIdAsync(SourceType.Receivable, MappingKeys.Revenue);

            if (arAccountId <= 0 || revenueAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Receivable posting.");

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine
                {
                    AccountId = arAccountId,
                    Debit = receivable.Amount,
                    Credit = 0,
                    PartnerId = receivable.PartnerId,
                    Description = receivable.Description ?? "Account Receivable"
                },
                new JournalEntryLine
                {
                    AccountId = revenueAccountId,
                    Debit = 0,
                    Credit = receivable.Amount,
                    PartnerId = receivable.PartnerId,
                    Description = receivable.Description ?? "Revenue"
                }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
