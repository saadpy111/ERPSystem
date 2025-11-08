using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class EmployeeContractRepository : IEmployeeContractRepository
    {
        private readonly HrDbContext _context;

        public EmployeeContractRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeContract> AddAsync(EmployeeContract employeeContract)
        {
            await _context.EmployeeContracts.AddAsync(employeeContract);
            return employeeContract;
        }

        public async Task<EmployeeContract?> GetByIdAsync(int id)
        {
            return await _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .FirstOrDefaultAsync(ec => ec.Id == id);
        }

        public async Task<IEnumerable<EmployeeContract>> GetAllAsync()
        {
            return await _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .ToListAsync();
        }

        public void Update(EmployeeContract employeeContract)
        {
            _context.EmployeeContracts.Update(employeeContract);
        }

        public void Delete(EmployeeContract employeeContract)
        {
            _context.EmployeeContracts.Remove(employeeContract);
        }
    }
}