using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class LoanInstallmentRepository : ILoanInstallmentRepository
    {
        private readonly HrDbContext _context;

        public LoanInstallmentRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<LoanInstallment> AddAsync(LoanInstallment loanInstallment)
        {
            await _context.LoanInstallments.AddAsync(loanInstallment);
            return loanInstallment;
        }

        public async Task<LoanInstallment?> GetByIdAsync(int id)
        {
            return await _context.LoanInstallments
                .Include(li => li.Loan)
                .FirstOrDefaultAsync(li => li.InstallmentId == id);
        }

        public async Task<IEnumerable<LoanInstallment>> GetAllAsync()
        {
            return await _context.LoanInstallments
                .Include(li => li.Loan)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanInstallment>> GetByLoanIdAsync(int loanId)
        {
            return await _context.LoanInstallments
                .Where(li => li.LoanId == loanId)
                .Include(li => li.Loan)
                .ToListAsync();
        }

        public void Update(LoanInstallment loanInstallment)
        {
            _context.LoanInstallments.Update(loanInstallment);
        }

        public void Delete(LoanInstallment loanInstallment)
        {
            _context.LoanInstallments.Remove(loanInstallment);
        }
    }
}
