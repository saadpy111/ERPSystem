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
    public class ProcurementAttachmentRepository : IProcurementAttachmentRepository
    {
        private readonly ProcurementDbContext _context;
        
        public ProcurementAttachmentRepository(ProcurementDbContext context)
        {
            _context = context;
        }
        
        public async Task<ProcurementAttachment> AddAsync(ProcurementAttachment attachment)
        {
            await _context.ProcurementAttachments.AddAsync(attachment);
            return attachment;
        }
        
        public async Task<ProcurementAttachment> GetByIdAsync(Guid id)
        {
            return await _context.ProcurementAttachments
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        
        public async Task<IEnumerable<ProcurementAttachment>> GetAllAsync()
        {
            return await _context.ProcurementAttachments
                .ToListAsync();
        }
        
        public async Task<IEnumerable<ProcurementAttachment>> GetAllByEntityAsync(string entityType, Guid entityId)
        {
            return await _context.ProcurementAttachments
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .ToListAsync();
        }
        
        public void Update(ProcurementAttachment attachment)
        {
            _context.ProcurementAttachments.Update(attachment);
        }
        
        public void Delete(ProcurementAttachment attachment)
        {
            _context.ProcurementAttachments.Remove(attachment);
        }
    }
}