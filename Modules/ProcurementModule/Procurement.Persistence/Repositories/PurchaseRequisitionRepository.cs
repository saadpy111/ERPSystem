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
    public class PurchaseRequisitionRepository : IPurchaseRequisitionRepository
    {
        private readonly ProcurementDbContext _context;
        
        public PurchaseRequisitionRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<PurchaseRequisition> AddAsync(PurchaseRequisition requisition)
        {
            await _context.PurchaseRequisitions.AddAsync(requisition);
            return requisition;
        }
        
        public async Task<PurchaseRequisition> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseRequisitions
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }
        
        public async Task<IEnumerable<PurchaseRequisition>> GetAllAsync()
        {
            return await _context.PurchaseRequisitions
                .ToListAsync();
        }
        
        public void Update(PurchaseRequisition requisition)
        {
            _context.PurchaseRequisitions.Update(requisition);
        }
        
        public void Delete(PurchaseRequisition requisition)
        {
            _context.PurchaseRequisitions.Remove(requisition);
        }
    }
}