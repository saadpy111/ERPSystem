using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace Procurement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProcurementDbContext _context;
        
        private IVendorRepository? _vendorRepository;
        private IPurchaseOrderRepository? _purchaseOrderRepository;
        private IPurchaseOrderItemRepository? _purchaseOrderItemRepository;
        private IGoodsReceiptRepository? _goodsReceiptRepository;
        private IPurchaseInvoiceRepository? _purchaseInvoiceRepository;
        private IPurchaseRequisitionRepository? _purchaseRequisitionRepository;
        private IProcurementAttachmentRepository? _procurementAttachmentRepository;
        
        public UnitOfWork(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public IVendorRepository VendorRepository => 
            _vendorRepository ??= new VendorRepository(_context);
            
        public IPurchaseOrderRepository PurchaseOrderRepository => 
            _purchaseOrderRepository ??= new PurchaseOrderRepository(_context);
            
        public IPurchaseOrderItemRepository PurchaseOrderItemRepository => 
            _purchaseOrderItemRepository ??= new PurchaseOrderItemRepository(_context);
            
        public IGoodsReceiptRepository GoodsReceiptRepository => 
            _goodsReceiptRepository ??= new GoodsReceiptRepository(_context);
            
        public IPurchaseInvoiceRepository PurchaseInvoiceRepository => 
            _purchaseInvoiceRepository ??= new PurchaseInvoiceRepository(_context);
            
        public IPurchaseRequisitionRepository PurchaseRequisitionRepository => 
            _purchaseRequisitionRepository ??= new PurchaseRequisitionRepository(_context);
        
        public IProcurementAttachmentRepository ProcurementAttachmentRepository => 
            _procurementAttachmentRepository ??= new ProcurementAttachmentRepository(_context);
        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}