using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly HrDbContext _context;

        public LoanRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Loan> AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            return loan;
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Employee)
                .Include(l => l.Installments)
                .FirstOrDefaultAsync(l => l.LoanId == id);
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Loans
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee)
                .Include(l => l.Installments)
                .ToListAsync();
        }

        public void Update(Loan loan)
        {
            _context.Loans.Update(loan);
        }

        public void Delete(Loan loan)
        {
            _context.Loans.Remove(loan);
        }
    }
}
