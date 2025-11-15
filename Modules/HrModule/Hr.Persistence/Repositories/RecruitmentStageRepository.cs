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
    public class RecruitmentStageRepository : IRecruitmentStageRepository
    {
        private readonly HrDbContext _context;

        public RecruitmentStageRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentStage> AddAsync(RecruitmentStage recruitmentStage)
        {
            await _context.RecruitmentStages.AddAsync(recruitmentStage);
            return recruitmentStage;
        }

        public async Task<RecruitmentStage?> GetByIdAsync(int id)
        {
            return await _context.RecruitmentStages
                .FirstOrDefaultAsync(rs => rs.StageId == id);
        }

        public async Task<IEnumerable<RecruitmentStage>> GetAllAsync()
        {
            return await _context.RecruitmentStages
                .OrderBy(rs => rs.SequenceOrder)
                .ToListAsync();
        }

        public async Task<PagedResult<RecruitmentStage>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.RecruitmentStages
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(rs => rs.Name.ToLower().Contains(searchTermLower));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<RecruitmentStage>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<RecruitmentStage> ApplyOrdering(IQueryable<RecruitmentStage> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "SequenceOrder";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(rs => rs.Name) : query.OrderBy(rs => rs.Name),
                "sequenceorder" => isDescending ? query.OrderByDescending(rs => rs.SequenceOrder) : query.OrderBy(rs => rs.SequenceOrder),
                _ => isDescending ? query.OrderByDescending(rs => rs.SequenceOrder) : query.OrderBy(rs => rs.SequenceOrder)
            };

            return query;
        }

        public void Update(RecruitmentStage recruitmentStage)
        {
            _context.RecruitmentStages.Update(recruitmentStage);
        }

        public void Delete(RecruitmentStage recruitmentStage)
        {
            _context.RecruitmentStages.Remove(recruitmentStage);
        }
    }
}