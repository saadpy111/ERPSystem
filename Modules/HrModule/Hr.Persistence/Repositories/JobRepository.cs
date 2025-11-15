using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hr.Persistence.Context;

namespace Hr.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly HrDbContext _context;

        public JobRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            return job;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.Department)
                .Include(j => j.Applicants)
                .FirstOrDefaultAsync(j => j.JobId == id);
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Include(j => j.Department)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Jobs
                .Where(j => j.DepartmentId == departmentId)
                .Include(j => j.Department)
                .ToListAsync();
        }

        public async Task<PagedResult<Job>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? departmentId = null, string? status = null, string? workType = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.Jobs
                .Include(j => j.Department)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(j => j.Title.ToLower().Contains(searchTermLower));
            }

            // Apply department filter
            if (departmentId.HasValue)
            {
                query = query.Where(j => j.DepartmentId == departmentId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<JobStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(j => j.Status == statusEnum);
                }
            }

            // Apply work type filter
            if (!string.IsNullOrWhiteSpace(workType))
            {
                if (Enum.TryParse<WorkType>(workType, true, out var workTypeEnum))
                {
                    query = query.Where(j => j.WorkType == workTypeEnum);
                }
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Job>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<Job> ApplyOrdering(IQueryable<Job> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Title";

            query = orderBy.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title),
                "publisheddate" => isDescending ? query.OrderByDescending(j => j.PublishedDate) : query.OrderBy(j => j.PublishedDate),
                "status" => isDescending ? query.OrderByDescending(j => j.Status) : query.OrderBy(j => j.Status),
                _ => isDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title)
            };

            return query;
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }

        public void Delete(Job job)
        {
            _context.Jobs.Remove(job);
        }
    }
}