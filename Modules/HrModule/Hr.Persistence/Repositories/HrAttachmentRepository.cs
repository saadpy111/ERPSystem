using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
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

        public async Task<PagedResult<HrAttachment>> GetPagedAsync(int pageNumber, int pageSize, string? entityType = null, int? entityId = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.Attachments
                .AsQueryable();

            // Apply entity type filter
            if (!string.IsNullOrWhiteSpace(entityType))
            {
                query = query.Where(a => a.EntityType == entityType);
            }

            // Apply entity ID filter
            if (entityId.HasValue)
            {
                query = query.Where(a => a.EntityId == entityId.Value);
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<HrAttachment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<HrAttachment> ApplyOrdering(IQueryable<HrAttachment> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "UploadedAt";

            query = orderBy.ToLower() switch
            {
                "filename" => isDescending ? query.OrderByDescending(a => a.FileName) : query.OrderBy(a => a.FileName),
                "uploadedat" => isDescending ? query.OrderByDescending(a => a.UploadedAt) : query.OrderBy(a => a.UploadedAt),
                "filesize" => isDescending ? query.OrderByDescending(a => a.FileSize) : query.OrderBy(a => a.FileSize),
                _ => isDescending ? query.OrderByDescending(a => a.UploadedAt) : query.OrderBy(a => a.UploadedAt)
            };

            return query;
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