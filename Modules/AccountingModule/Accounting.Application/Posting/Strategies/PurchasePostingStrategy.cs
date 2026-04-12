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
    public class PurchasePostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchasePostingStrategy(IAccountingMappingService mappingService, IPurchaseRepository purchaseRepository)
        {
            _mappingService = mappingService;
            _purchaseRepository = purchaseRepository;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Purchases;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(request.SourceId);
            if (purchase == null)
                return Result<List<JournalEntryLine>>.Failure($"Purchase with ID {request.SourceId} not found.");

            var inventoryAccountId = await _mappingService.GetAccountIdAsync(SourceType.Purchases, "Inventory");
            var payableAccountId = await _mappingService.GetAccountIdAsync(SourceType.Purchases, "AccountsPayable");

            if (inventoryAccountId <= 0 || payableAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Missing account mapping for Purchase posting.");

            var lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = inventoryAccountId, Debit = purchase.TotalAmount, Credit = 0, Description = "Inventory Receipt" },
                new JournalEntryLine { AccountId = payableAccountId, Debit = 0, Credit = purchase.TotalAmount, Description = "Accounts Payable" }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
