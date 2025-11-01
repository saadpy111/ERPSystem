using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class PayrollRecordRepository : IPayrollRecordRepository
    {
        private readonly HrDbContext _context;

        public PayrollRecordRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<PayrollRecord> AddAsync(PayrollRecord payrollRecord)
        {
            await _context.PayrollRecords.AddAsync(payrollRecord);
            return payrollRecord;
        }

        public async Task<PayrollRecord?> GetByIdAsync(int id)
        {
            return await _context.PayrollRecords
                .Include(pr => pr.Employee)
                .Include(pr => pr.Components)
                .FirstOrDefaultAsync(pr => pr.PayrollId == id);
        }

        public async Task<IEnumerable<PayrollRecord>> GetAllAsync()
        {
            return await _context.PayrollRecords
                .Include(pr => pr.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollRecord>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.PayrollRecords
                .Where(pr => pr.EmployeeId == employeeId)
                .Include(pr => pr.Employee)
                .Include(pr => pr.Components)
                .ToListAsync();
        }

        public void Update(PayrollRecord payrollRecord)
        {
            _context.PayrollRecords.Update(payrollRecord);
        }

        public void Delete(PayrollRecord payrollRecord)
        {
            _context.PayrollRecords.Remove(payrollRecord);
        }
    }
}
