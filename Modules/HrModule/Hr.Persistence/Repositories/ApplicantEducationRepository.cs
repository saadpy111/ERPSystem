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
    public class ApplicantEducationRepository : IApplicantEducationRepository
    {
        private readonly HrDbContext _context;

        public ApplicantEducationRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicantEducation> AddAsync(ApplicantEducation applicantEducation)
        {
            await _context.ApplicantEducations.AddAsync(applicantEducation);
            return applicantEducation;
        }

        public async Task<ApplicantEducation?> GetByIdAsync(int id)
        {
            return await _context.ApplicantEducations.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicantEducation>> GetAllAsync()
        {
            return await _context.ApplicantEducations.ToListAsync();
        }

        public async Task<IEnumerable<ApplicantEducation>> GetByApplicantIdAsync(int applicantId)
        {
            return await _context.ApplicantEducations
                .Where(ae => ae.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task RemoveEducations(int applicantId)
        {
            var educations = await _context.ApplicantEducations
                .Where(ae => ae.ApplicantId == applicantId)
                .ToListAsync();
            
            _context.ApplicantEducations.RemoveRange(educations);
        }

        public async Task AddEducations(int applicantId, IEnumerable<ApplicantEducation> educations)
        {
            foreach (var education in educations)
            {
                education.ApplicantId = applicantId;
                await _context.ApplicantEducations.AddAsync(education);
            }
        }

        public async Task<PagedResult<ApplicantEducation>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.ApplicantEducations.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(ae => 
                    ae.DegreeName.ToLower().Contains(searchTermLower) ||
                    (ae.Institute != null && ae.Institute.ToLower().Contains(searchTermLower)) ||
                    (ae.Specialization != null && ae.Specialization.ToLower().Contains(searchTermLower)));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ApplicantEducation>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<ApplicantEducation> ApplyOrdering(IQueryable<ApplicantEducation> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Institute";

            query = orderBy.ToLower() switch
            {
                "degreename" => isDescending ? query.OrderByDescending(ae => ae.DegreeName) : query.OrderBy(ae => ae.DegreeName),
                "institute" => isDescending ? query.OrderByDescending(ae => ae.Institute) : query.OrderBy(ae => ae.Institute),
                "graduationyear" => isDescending ? query.OrderByDescending(ae => ae.GraduationYear) : query.OrderBy(ae => ae.GraduationYear),
                _ => isDescending ? query.OrderByDescending(ae => ae.Institute) : query.OrderBy(ae => ae.Institute)
            };

            return query;
        }

        public void Update(ApplicantEducation applicantEducation)
        {
            _context.ApplicantEducations.Update(applicantEducation);
        }

        public void Delete(ApplicantEducation applicantEducation)
        {
            _context.ApplicantEducations.Remove(applicantEducation);
        }
    }
}