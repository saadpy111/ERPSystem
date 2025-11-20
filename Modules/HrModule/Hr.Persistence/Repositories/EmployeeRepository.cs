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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HrDbContext _context;

        public EmployeeRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return employee;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                         .Include(e=>e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }
   
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId)
        {
            // This functionality is now handled through EmployeeContract
            return await _context.Employees.ToListAsync();
        }

        public async Task<PagedResult<Employee>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false, string? status = null)
        {
            var query = _context.Employees
                .Include(e => e.Manager)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(e => e.FullName.ToLower().Contains(searchTermLower) || 
                                       e.Email.ToLower().Contains(searchTermLower) ||
                                       (e.PhoneNumber != null && e.PhoneNumber.ToLower().Contains(searchTermLower)));
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<EmployeeStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(e => e.Status == statusEnum);
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

            return new PagedResult<Employee>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<Employee> ApplyOrdering(IQueryable<Employee> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "FullName";

            query = orderBy.ToLower() switch
            {
                "fullname" => isDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName),
                "email" => isDescending ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                "status" => isDescending ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                _ => isDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName)
            };

            return query;
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
        }
    }
}