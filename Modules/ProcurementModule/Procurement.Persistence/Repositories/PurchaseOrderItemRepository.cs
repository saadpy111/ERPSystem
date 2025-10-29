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
    public class PurchaseOrderItemRepository : IPurchaseOrderItemRepository
    {
        private readonly ProcurementDbContext _context;
        
        public PurchaseOrderItemRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<PurchaseOrderItem> AddAsync(PurchaseOrderItem item)
        {
            await _context.PurchaseOrderItems.AddAsync(item);
            return item;
        }
        
        public async Task<PurchaseOrderItem> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrderItems
                .Include(poi => poi.PurchaseOrder)
                .FirstOrDefaultAsync(poi => poi.Id == id);
        }
        
        public async Task<IEnumerable<PurchaseOrderItem>> GetAllAsync()
        {
            return await _context.PurchaseOrderItems
                .Include(poi => poi.PurchaseOrder)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .Where(poi => poi.PurchaseOrderId == purchaseOrderId)
                .Include(poi => poi.PurchaseOrder)
                .ToListAsync();
        }
        
        public void Update(PurchaseOrderItem item)
        {
            _context.PurchaseOrderItems.Update(item);
        }
        
        public void Delete(PurchaseOrderItem item)
        {
            _context.PurchaseOrderItems.Remove(item);
        }
    }
}