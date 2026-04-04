using Accounting.Application.Interfaces.External;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    public class InventoryPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryPostingStrategy(IAccountingMappingService mappingService, IInventoryRepository inventoryRepository)
        {
            _mappingService = mappingService;
            _inventoryRepository = inventoryRepository;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Inventory;

        public async Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest request)
        {
            var move = await _inventoryRepository.GetByIdAsync(request.SourceId);

            var cogsAccountId = await _mappingService.GetAccountIdAsync(SourceType.Inventory, "COGS");
            var inventoryAccountId = await _mappingService.GetAccountIdAsync(SourceType.Inventory, "Inventory");

            return new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = cogsAccountId, Debit = move.TotalCost, Credit = 0, Description = "Cost of Goods Sold" },
                new JournalEntryLine { AccountId = inventoryAccountId, Debit = 0, Credit = move.TotalCost, Description = "Inventory Deduction" }
            };
        }
    }
}
