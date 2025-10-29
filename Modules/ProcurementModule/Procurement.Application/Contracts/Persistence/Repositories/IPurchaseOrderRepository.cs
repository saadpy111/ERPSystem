using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> AddAsync(PurchaseOrder purchaseOrder);
        Task<PurchaseOrder> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrder>> GetAllAsync();
        Task<IEnumerable<PurchaseOrder>> GetByVendorIdAsync(Guid vendorId);
        Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status);
        void Update(PurchaseOrder purchaseOrder);
        void Delete(PurchaseOrder purchaseOrder);
    }
}