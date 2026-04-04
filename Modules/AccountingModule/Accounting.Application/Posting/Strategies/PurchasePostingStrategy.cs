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

        public async Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest request)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(request.SourceId);

            var inventoryAccountId = await _mappingService.GetAccountIdAsync(SourceType.Purchases, "Inventory");
            var payableAccountId = await _mappingService.GetAccountIdAsync(SourceType.Purchases, "AccountsPayable");

            return new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = inventoryAccountId, Debit = purchase.TotalAmount, Credit = 0, Description = "Inventory Receipt" },
                new JournalEntryLine { AccountId = payableAccountId, Debit = 0, Credit = purchase.TotalAmount, Description = "Accounts Payable" }
            };
        }
    }
}
