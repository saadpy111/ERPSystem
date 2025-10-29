using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IGoodsReceiptRepository
    {
        Task<GoodsReceipt> AddAsync(GoodsReceipt goodsReceipt);
        Task<GoodsReceipt> GetByIdAsync(Guid id, bool includeItems = false);
        Task<IEnumerable<GoodsReceipt>> GetAllAsync();
        Task<IEnumerable<GoodsReceipt>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId);
        void Update(GoodsReceipt goodsReceipt);
        void Delete(GoodsReceipt goodsReceipt);
    }
}