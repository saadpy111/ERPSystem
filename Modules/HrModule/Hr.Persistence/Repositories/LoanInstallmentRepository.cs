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
                    .ThenInclude(l => l.Employee)
                .FirstOrDefaultAsync(li => li.InstallmentId == id);
        }

        public async Task<IEnumerable<LoanInstallment>> GetAllAsync()
        {
            return await _context.LoanInstallments
                .Include(li => li.Loan)
                    .ThenInclude(l => l.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanInstallment>> GetByLoanIdAsync(int loanId)
        {
            return await _context.LoanInstallments
                .Where(li => li.LoanId == loanId)
                .Include(li => li.Loan)
                    .ThenInclude(l => l.Employee)
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

        public async Task<IEnumerable<LoanInstallment>> GetAllPendingAsync()
        {
            return await _context.LoanInstallments
                .Include(li => li.Loan)
                    .ThenInclude(l => l.Employee)
                .Where(li=>li.Status == Domain.Enums.InstallmentStatus.Pending)
                .ToListAsync();
        }

        public async Task<PagedResult<LoanInstallment>> GetPagedAsync(int pageNumber, int pageSize, int? loanId = null, string? status = null, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.LoanInstallments
                .Include(li => li.Loan)
                    .ThenInclude(l => l.Employee)
                .AsQueryable();

            // Apply loan filter
            if (loanId.HasValue)
            {
                query = query.Where(li => li.LoanId == loanId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<InstallmentStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(li => li.Status == statusEnum);
                }
            }

            // Apply search term filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(li =>
                    li.Loan.Employee.FullName.ToLower().Contains(searchTermLower) );
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LoanInstallment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<LoanInstallment> ApplyOrdering(IQueryable<LoanInstallment> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "DueDate";

            query = orderBy.ToLower() switch
            {
                "duedate" => isDescending ? query.OrderByDescending(li => li.DueDate) : query.OrderBy(li => li.DueDate),
                "amountdue" => isDescending ? query.OrderByDescending(li => li.AmountDue) : query.OrderBy(li => li.AmountDue),
                "paymentdate" => isDescending ? query.OrderByDescending(li => li.PaymentDate) : query.OrderBy(li => li.PaymentDate),
                "status" => isDescending ? query.OrderByDescending(li => li.Status) : query.OrderBy(li => li.Status),
                _ => isDescending ? query.OrderByDescending(li => li.DueDate) : query.OrderBy(li => li.DueDate)
            };

            return query;
        }
    }
}