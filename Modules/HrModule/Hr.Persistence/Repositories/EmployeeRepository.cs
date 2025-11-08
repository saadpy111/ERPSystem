using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
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
