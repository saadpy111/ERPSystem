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
    public class GoodsReceiptRepository : IGoodsReceiptRepository
    {
        private readonly ProcurementDbContext _context;

        public GoodsReceiptRepository(ProcurementDbContext context)
        {
            _context = context;
        }

        public async Task<GoodsReceipt> AddAsync(GoodsReceipt goodsReceipt)
        {
            await _context.GoodsReceipts.AddAsync(goodsReceipt);
            return goodsReceipt;
        }

        public async Task<GoodsReceipt?> GetByIdAsync(Guid id, bool includeItems = false)
        {
            IQueryable<GoodsReceipt> query = _context.GoodsReceipts
                .Include(gr => gr.PurchaseOrder);

            if (includeItems)
                query = query.Include(gr => gr.Items);

            return await query.FirstOrDefaultAsync(gr => gr.Id == id);
        }

        public async Task<IEnumerable<GoodsReceipt>> GetAllAsync()
        {
            return await _context.GoodsReceipts
                .Include(gr => gr.PurchaseOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId)
        {
            return await _context.GoodsReceipts
                .Where(gr => gr.PurchaseOrderId == purchaseOrderId)
                .Include(gr => gr.PurchaseOrder)
                .ToListAsync();
        }

        public void Update(GoodsReceipt goodsReceipt)
        {
            _context.GoodsReceipts.Update(goodsReceipt);
        }

        public void Delete(GoodsReceipt goodsReceipt)
        {
            _context.GoodsReceipts.Remove(goodsReceipt);
        }
    }
}
