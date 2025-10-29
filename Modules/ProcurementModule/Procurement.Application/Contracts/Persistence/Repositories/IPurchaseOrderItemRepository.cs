using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IPurchaseOrderItemRepository
    {
        Task<PurchaseOrderItem> AddAsync(PurchaseOrderItem item);
        Task<PurchaseOrderItem> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderItem>> GetAllAsync();
        Task<IEnumerable<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId);
        void Update(PurchaseOrderItem item);
        void Delete(PurchaseOrderItem item);
    }
}