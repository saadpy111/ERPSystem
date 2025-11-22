using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly HrDbContext _context;

        public ApplicantRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant> AddAsync(Applicant applicant)
        {
            await _context.Applicants.AddAsync(applicant);
            return applicant;
        }

        public async Task<Applicant?> GetByIdAsync(int id)
        {
            return await _context.Applicants
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .Include(a => a.Educations)
                .Include(a => a.Experiences)
                .FirstOrDefaultAsync(a => a.ApplicantId == id);
        }

        public async Task<IEnumerable<Applicant>> GetAllAsync()
        {
            return await _context.Applicants
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .Include(a => a.Educations)
                .Include(a => a.Experiences)
                .ToListAsync();
        }

        public async Task<IEnumerable<Applicant>> GetByJobIdAsync(int jobId)
        {
            return await _context.Applicants
                .Where(a => a.JobId == jobId)
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .Include(a => a.Educations)
                .Include(a => a.Experiences)
                .ToListAsync();
        }

        public async Task<PagedResult<Applicant>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? jobId = null, int? currentStageId = null, string? status = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.Applicants
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .Include(a => a.Educations)
                .Include(a => a.Experiences)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(a => a.FullName.ToLower().Contains(searchTermLower));
            }

            // Apply job filter
            if (jobId.HasValue)
            {
                query = query.Where(a => a.JobId == jobId.Value);
            }

            // Apply stage filter
            if (currentStageId.HasValue)
            {
                query = query.Where(a => a.CurrentStageId == currentStageId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<ApplicantStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(a => a.Status == statusEnum);
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

            return new PagedResult<Applicant>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<Applicant> ApplyOrdering(IQueryable<Applicant> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "FullName";

            query = orderBy.ToLower() switch
            {
                "fullname" => isDescending ? query.OrderByDescending(a => a.FullName) : query.OrderBy(a => a.FullName),
                "applicationdate" => isDescending ? query.OrderByDescending(a => a.ApplicationDate) : query.OrderBy(a => a.ApplicationDate),
                "status" => isDescending ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status),
                _ => isDescending ? query.OrderByDescending(a => a.FullName) : query.OrderBy(a => a.FullName)
            };

            return query;
        }

        public void Update(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
        }

        public void Delete(Applicant applicant)
        {
            _context.Applicants.Remove(applicant);
        }
    }
}