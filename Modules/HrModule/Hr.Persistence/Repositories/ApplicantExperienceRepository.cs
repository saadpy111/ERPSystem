using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class ApplicantExperienceRepository : IApplicantExperienceRepository
    {
        private readonly HrDbContext _context;

        public ApplicantExperienceRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicantExperience> AddAsync(ApplicantExperience applicantExperience)
        {
            await _context.ApplicantExperiences.AddAsync(applicantExperience);
            return applicantExperience;
        }

        public async Task<ApplicantExperience?> GetByIdAsync(int id)
        {
            return await _context.ApplicantExperiences.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicantExperience>> GetAllAsync()
        {
            return await _context.ApplicantExperiences.ToListAsync();
        }

        public async Task<IEnumerable<ApplicantExperience>> GetByApplicantIdAsync(int applicantId)
        {
            return await _context.ApplicantExperiences
                .Where(ae => ae.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task RemoveExperiences(int applicantId)
        {
            var experiences = await _context.ApplicantExperiences
                .Where(ae => ae.ApplicantId == applicantId)
                .ToListAsync();
            
            _context.ApplicantExperiences.RemoveRange(experiences);
        }

        public async Task AddExperiences(int applicantId, IEnumerable<ApplicantExperience> experiences)
        {
            foreach (var experience in experiences)
            {
                experience.ApplicantId = applicantId;
                await _context.ApplicantExperiences.AddAsync(experience);
            }
        }

        public async Task<PagedResult<ApplicantExperience>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.ApplicantExperiences.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(ae => 
                    ae.CompanyName.ToLower().Contains(searchTermLower) ||
                    ae.JobTitle.ToLower().Contains(searchTermLower) ||
                    (ae.Description != null && ae.Description.ToLower().Contains(searchTermLower)));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ApplicantExperience>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<ApplicantExperience> ApplyOrdering(IQueryable<ApplicantExperience> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "CompanyName";

            query = orderBy.ToLower() switch
            {
                "companyname" => isDescending ? query.OrderByDescending(ae => ae.CompanyName) : query.OrderBy(ae => ae.CompanyName),
                "jobtitle" => isDescending ? query.OrderByDescending(ae => ae.JobTitle) : query.OrderBy(ae => ae.JobTitle),
                "startdate" => isDescending ? query.OrderByDescending(ae => ae.StartDate) : query.OrderBy(ae => ae.StartDate),
                _ => isDescending ? query.OrderByDescending(ae => ae.CompanyName) : query.OrderBy(ae => ae.CompanyName)
            };

            return query;
        }

        public void Update(ApplicantExperience applicantExperience)
        {
            _context.ApplicantExperiences.Update(applicantExperience);
        }

        public void Delete(ApplicantExperience applicantExperience)
        {
            _context.ApplicantExperiences.Remove(applicantExperience);
        }
    }
}