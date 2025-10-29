using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IPurchaseInvoiceRepository
    {
        Task<PurchaseInvoice> AddAsync(PurchaseInvoice invoice);
        Task<PurchaseInvoice> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseInvoice>> GetAllAsync();
        Task<IEnumerable<PurchaseInvoice>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId);
        void Update(PurchaseInvoice invoice);
        void Delete(PurchaseInvoice invoice);
    }
}