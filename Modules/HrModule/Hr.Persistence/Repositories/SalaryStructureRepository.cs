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
    public class SalaryStructureRepository : ISalaryStructureRepository
    {
        private readonly HrDbContext _context;

        public SalaryStructureRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<SalaryStructure> AddAsync(SalaryStructure salaryStructure)
        {
            await _context.SalaryStructures.AddAsync(salaryStructure);
            return salaryStructure;
        }

        public async Task<SalaryStructure?> GetByIdAsync(int id)
        {
            return await _context.SalaryStructures
                .Include(s => s.Components)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SalaryStructure>> GetAllAsync()
        {
            return await _context.SalaryStructures
                .Include(s => s.Components)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryStructure>> GetActiveAsync()
        {
            return await _context.SalaryStructures
                .Include(s => s.Components)
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<PagedResult<SalaryStructure>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.SalaryStructures
                .Include(s => s.Components)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(searchTermLower));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<SalaryStructure>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<SalaryStructure> ApplyOrdering(IQueryable<SalaryStructure> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Name";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "effectivedate" => isDescending ? query.OrderByDescending(s => s.EffectiveDate) : query.OrderBy(s => s.EffectiveDate),
                "isactive" => isDescending ? query.OrderByDescending(s => s.IsActive) : query.OrderBy(s => s.IsActive),
                _ => isDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name)
            };

            return query;
        }

        public void Update(SalaryStructure salaryStructure)
        {
            _context.SalaryStructures.Update(salaryStructure);
        }

        public void Delete(SalaryStructure salaryStructure)
        {
            _context.SalaryStructures.Remove(salaryStructure);
        }
    }
}