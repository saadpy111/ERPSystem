using System;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IVendorRepository VendorRepository { get; }
        IPurchaseOrderRepository PurchaseOrderRepository { get; }
        IPurchaseOrderItemRepository PurchaseOrderItemRepository { get; }
        IGoodsReceiptRepository GoodsReceiptRepository { get; }
        IPurchaseInvoiceRepository PurchaseInvoiceRepository { get; }
        IPurchaseRequisitionRepository PurchaseRequisitionRepository { get; }
        
        Task<int> SaveChangesAsync();
    }
}