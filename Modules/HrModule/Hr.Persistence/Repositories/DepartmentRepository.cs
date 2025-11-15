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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HrDbContext _context;

        public DepartmentRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Department> AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            return department;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Jobs)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Jobs)
                .ToListAsync();
        }

        public async Task<PagedResult<Department>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Jobs)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(d => 
                    d.Name.ToLower().Contains(searchTermLower) ||
                    (d.Description != null && d.Description.ToLower().Contains(searchTermLower)));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Department>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<Department> ApplyOrdering(IQueryable<Department> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Name";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                _ => isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name)
            };

            return query;
        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
        }

        public void Delete(Department department)
        {
            _context.Departments.Remove(department);
        }
    }
}