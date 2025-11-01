using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class PayrollComponentRepository : IPayrollComponentRepository
    {
        private readonly HrDbContext _context;

        public PayrollComponentRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<PayrollComponent> AddAsync(PayrollComponent payrollComponent)
        {
            await _context.PayrollComponents.AddAsync(payrollComponent);
            return payrollComponent;
        }

        public async Task<PayrollComponent?> GetByIdAsync(int id)
        {
            return await _context.PayrollComponents
                .Include(pc => pc.PayrollRecord)
                .FirstOrDefaultAsync(pc => pc.ComponentId == id);
        }

        public async Task<IEnumerable<PayrollComponent>> GetAllAsync()
        {
            return await _context.PayrollComponents
                .Include(pc => pc.PayrollRecord)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollComponent>> GetByPayrollRecordIdAsync(int payrollRecordId)
        {
            return await _context.PayrollComponents
                .Where(pc => pc.PayrollRecordId == payrollRecordId)
                .Include(pc => pc.PayrollRecord)
                .ToListAsync();
        }

        public void Update(PayrollComponent payrollComponent)
        {
            _context.PayrollComponents.Update(payrollComponent);
        }

        public void Delete(PayrollComponent payrollComponent)
        {
            _context.PayrollComponents.Remove(payrollComponent);
        }
    }
}
