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
    public class VendorRepository : IVendorRepository
    {
        private readonly ProcurementDbContext _context;
        
        public VendorRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<Vendor> AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            return vendor;
        }
        
        public async Task<Vendor> GetByIdAsync(Guid id)
        {
            return await _context.Vendors
                .Include(v => v.PurchaseOrders)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        
        public async Task<IEnumerable<Vendor>> GetAllAsync()
        {
            return await _context.Vendors
                .Include(v => v.PurchaseOrders)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Vendor>> GetActiveVendorsAsync()
        {
            return await _context.Vendors
                .Where(v => v.IsActive)
                .Include(v => v.PurchaseOrders)
                .ToListAsync();
        }
        
        public void Update(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
        }
        
        public void Delete(Vendor vendor)
        {
            _context.Vendors.Remove(vendor);
        }
    }
}