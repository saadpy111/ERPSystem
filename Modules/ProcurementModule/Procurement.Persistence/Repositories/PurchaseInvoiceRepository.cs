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
    public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
    {
        private readonly ProcurementDbContext _context;
        
        public PurchaseInvoiceRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<PurchaseInvoice> AddAsync(PurchaseInvoice invoice)
        {
            await _context.PurchaseInvoices.AddAsync(invoice);
            return invoice;
        }
        
        public async Task<PurchaseInvoice> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseInvoices
                .Include(pi => pi.PurchaseOrder)
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }
        
        public async Task<IEnumerable<PurchaseInvoice>> GetAllAsync()
        {
            return await _context.PurchaseInvoices
                .Include(pi => pi.PurchaseOrder)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<PurchaseInvoice>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseInvoices
                .Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                .Include(pi => pi.PurchaseOrder)
                .ToListAsync();
        }
        
        public void Update(PurchaseInvoice invoice)
        {
            _context.PurchaseInvoices.Update(invoice);
        }
        
        public void Delete(PurchaseInvoice invoice)
        {
            _context.PurchaseInvoices.Remove(invoice);
        }
    }
}