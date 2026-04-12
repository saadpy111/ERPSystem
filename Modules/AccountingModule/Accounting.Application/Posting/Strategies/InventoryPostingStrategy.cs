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

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var move = await _inventoryRepository.GetByIdAsync(request.SourceId);
            if (move == null)
                return Result<List<JournalEntryLine>>.Failure($"Inventory move with ID {request.SourceId} not found.");

            var cogsAccountId = await _mappingService.GetAccountIdAsync(SourceType.Inventory, "COGS");
            var inventoryAccountId = await _mappingService.GetAccountIdAsync(SourceType.Inventory, "Inventory");

            if (cogsAccountId <= 0 || inventoryAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Inventory posting.");

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = cogsAccountId, Debit = move.TotalCost, Credit = 0, Description = "Cost of Goods Sold" },
                new JournalEntryLine { AccountId = inventoryAccountId, Debit = 0, Credit = move.TotalCost, Description = "Inventory Deduction" }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
