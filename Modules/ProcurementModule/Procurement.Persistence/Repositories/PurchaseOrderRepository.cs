using Microsoft.EntityFrameworkCore;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Domain.Entities;
using Procurement.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procurement.Persistence.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ProcurementDbContext _context;
        
        public PurchaseOrderRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<PurchaseOrder> AddAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrders.AddAsync(purchaseOrder);
            return purchaseOrder;
        }
        
        public async Task<PurchaseOrder> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders
                .Include(po => po.Vendor)
                .Include(po => po.Items)
                .Include(po => po.GoodsReceipts)
                .Include(po => po.Invoices)
                .FirstOrDefaultAsync(po => po.Id == id);
        }
        
        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                .Include(po => po.Vendor)
                .Include(po => po.Items)
                .Include(po => po.GoodsReceipts)
                .Include(po => po.Invoices)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<PurchaseOrder>> GetByVendorIdAsync(Guid vendorId)
        {
            return await _context.PurchaseOrders
                .Where(po => po.VendorId == vendorId)
                .Include(po => po.Vendor)
                .Include(po => po.Items)
                .Include(po => po.GoodsReceipts)
                .Include(po => po.Invoices)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status)
        {
            return await _context.PurchaseOrders
                .Where(po => po.Status == status)
                .Include(po => po.Vendor)
                .Include(po => po.Items)
                .Include(po => po.GoodsReceipts)
                .Include(po => po.Invoices)
                .ToListAsync();
        }
        
        public void Update(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
        }
        
        public void Delete(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Remove(purchaseOrder);
        }
    }
}