using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class SalaryStructureComponentRepository : ISalaryStructureComponentRepository
    {
        private readonly HrDbContext _context;

        public SalaryStructureComponentRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<SalaryStructureComponent> AddAsync(SalaryStructureComponent component)
        {
            await _context.SalaryStructureComponents.AddAsync(component);
            return component;
        }

        public async Task<SalaryStructureComponent?> GetByIdAsync(int id)
        {
            return await _context.SalaryStructureComponents
                .Include(c => c.SalaryStructure)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<SalaryStructureComponent>> GetAllAsync()
        {
            return await _context.SalaryStructureComponents
                .Include(c => c.SalaryStructure)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryStructureComponent>> GetBySalaryStructureIdAsync(int salaryStructureId)
        {
            return await _context.SalaryStructureComponents
                .Include(c => c.SalaryStructure)
                .Where(c => c.SalaryStructureId == salaryStructureId)
                .ToListAsync();
        }

        public void Update(SalaryStructureComponent component)
        {
            _context.SalaryStructureComponents.Update(component);
        }

        public void Delete(SalaryStructureComponent component)
        {
            _context.SalaryStructureComponents.Remove(component);
        }
    }
}