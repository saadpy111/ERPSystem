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
                    .ThenInclude(e => e.Contracts)
                    .ThenInclude(c => c.Job)
                .Include(l => l.Installments)
                .FirstOrDefaultAsync(l => l.LoanId == id);
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Employee)
                    .ThenInclude(e => e.Contracts)
                    .ThenInclude(c => c.Job)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Loans
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee)
                    .ThenInclude(e => e.Contracts)
                    .ThenInclude(c => c.Job)
                .Include(l => l.Installments)
                .ToListAsync();
        }

        public async Task<PagedResult<Loan>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, string? status = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.Loans
                .Include(l => l.Employee)
                    .ThenInclude(e => e.Contracts)
                    .ThenInclude(c => c.Job)
                .AsQueryable();

            // Apply employee filter
            if (employeeId.HasValue)
            {
                query = query.Where(l => l.EmployeeId == employeeId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<LoanStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(l => l.Status == statusEnum);
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

            return new PagedResult<Loan>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<Loan> ApplyOrdering(IQueryable<Loan> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "StartDate";

            query = orderBy.ToLower() switch
            {
                "principalamount" => isDescending ? query.OrderByDescending(l => l.PrincipalAmount) : query.OrderBy(l => l.PrincipalAmount),
                "startdate" => isDescending ? query.OrderByDescending(l => l.StartDate) : query.OrderBy(l => l.StartDate),
                "remainingbalance" => isDescending ? query.OrderByDescending(l => l.RemainingBalance) : query.OrderBy(l => l.RemainingBalance),
                "status" => isDescending ? query.OrderByDescending(l => l.Status) : query.OrderBy(l => l.Status),
                _ => isDescending ? query.OrderByDescending(l => l.StartDate) : query.OrderBy(l => l.StartDate)
            };

            return query;
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