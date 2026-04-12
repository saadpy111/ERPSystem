using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.External;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    public class SalesPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly ISalesRepository _salesRepository;

        public SalesPostingStrategy(IAccountingMappingService mappingService, ISalesRepository salesRepository)
        {
            _mappingService = mappingService;
            _salesRepository = salesRepository;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Sales;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var sales = await _salesRepository.GetByIdAsync(request.SourceId);
            if (sales == null)
                return Result<List<JournalEntryLine>>.Failure($"Sales with ID {request.SourceId} not found.");

            var cashAccountId = await _mappingService.GetAccountIdAsync(SourceType.Sales, "Cash");
            var revenueAccountId = await _mappingService.GetAccountIdAsync(SourceType.Sales, "Revenue");

            if (cashAccountId <= 0 || revenueAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Sales posting.");

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = cashAccountId, Debit = sales.TotalAmount, Credit = 0, Description = "Cash Receipt" },
                new JournalEntryLine { AccountId = revenueAccountId, Debit = 0, Credit = sales.TotalAmount, Description = "Sales Revenue" }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
