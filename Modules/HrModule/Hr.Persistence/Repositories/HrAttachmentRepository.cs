using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class HrAttachmentRepository : IHrAttachmentRepository
    {
        private readonly HrDbContext _context;

        public HrAttachmentRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<HrAttachment> AddAsync(HrAttachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            return attachment;
        }

        public async Task<HrAttachment?> GetByIdAsync(int id)
        {
            return await _context.Attachments
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<HrAttachment>> GetAllAsync()
        {
            return await _context.Attachments
                .ToListAsync();
        }

        public async Task<IEnumerable<HrAttachment>> GetByEntityAsync(string entityType, int entityId)
        {
            return await _context.Attachments
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .ToListAsync();
        }

        public void Update(HrAttachment attachment)
        {
            _context.Attachments.Update(attachment);
        }

        public void Delete(HrAttachment attachment)
        {
            _context.Attachments.Remove(attachment);
        }
    }
}