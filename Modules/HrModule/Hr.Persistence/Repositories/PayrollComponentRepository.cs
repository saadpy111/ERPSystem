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

        public async Task<PagedResult<PayrollComponent>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? payrollRecordId = null, string? componentType = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.PayrollComponents
                .Include(pc => pc.PayrollRecord)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(pc => pc.Name.ToLower().Contains(searchTermLower));
            }

            // Apply payroll record filter
            if (payrollRecordId.HasValue)
            {
                query = query.Where(pc => pc.PayrollRecordId == payrollRecordId.Value);
            }

            // Apply component type filter
            if (!string.IsNullOrWhiteSpace(componentType))
            {
                if (Enum.TryParse<PayrollComponentType>(componentType, true, out var componentTypeEnum))
                {
                    query = query.Where(pc => pc.ComponentType == componentTypeEnum);
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

            return new PagedResult<PayrollComponent>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<PayrollComponent> ApplyOrdering(IQueryable<PayrollComponent> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Name";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(pc => pc.Name) : query.OrderBy(pc => pc.Name),
                //"amount" => isDescending ? query.OrderByDescending(pc => pc.Amount) : query.OrderBy(pc => pc.Amount),
                "componenttype" => isDescending ? query.OrderByDescending(pc => pc.ComponentType) : query.OrderBy(pc => pc.ComponentType),
                _ => isDescending ? query.OrderByDescending(pc => pc.Name) : query.OrderBy(pc => pc.Name)
            };

            return query;
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