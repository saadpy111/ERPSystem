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

        public async Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest request)
        {
            var sales = await _salesRepository.GetByIdAsync(request.SourceId);
            
            var cashAccountId = await _mappingService.GetAccountIdAsync(SourceType.Sales, "Cash");
            var revenueAccountId = await _mappingService.GetAccountIdAsync(SourceType.Sales, "Revenue");

            return new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = cashAccountId, Debit = sales.TotalAmount, Credit = 0, Description = "Cash Receipt" },
                new JournalEntryLine { AccountId = revenueAccountId, Debit = 0, Credit = sales.TotalAmount, Description = "Sales Revenue" }
            };
        }
    }
}
