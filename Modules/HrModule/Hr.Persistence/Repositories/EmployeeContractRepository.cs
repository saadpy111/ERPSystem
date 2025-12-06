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
                .ThenInclude(e=>e.Manager)
                .Include(ec => ec.Job)
                .ThenInclude(j=>j.Department)
                .Include(ec => ec.SalaryStructure)
                .ThenInclude(s => s.Components)
                .FirstOrDefaultAsync(ec => ec.Id == id);
        }

        public async Task<EmployeeContract?> GetContractByEmployeeIdAsync(int id)
        {
            return await _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .Include(ec => ec.SalaryStructure)
                .ThenInclude(s =>s.Components)
                .FirstOrDefaultAsync(ec => ec.EmployeeId == id);
        }

        public async Task<IEnumerable<EmployeeContract>> GetContractsByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .Include(ec => ec.SalaryStructure)
                .ThenInclude(s => s.Components)
                .Where(ec => ec.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeContract>> GetAllAsync()
        {
            return await _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .Include(ec => ec.SalaryStructure)
                .ThenInclude(s => s.Components)
                .ToListAsync();
        }

        public async Task<PagedResult<EmployeeContract>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, int? jobId = null, string? contractType = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.EmployeeContracts
                .Include(ec => ec.Employee)
                .Include(ec => ec.Job)
                .Include(ec => ec.SalaryStructure)
                .ThenInclude(s => s.Components)
                .AsQueryable();

            // Apply employee filter
            if (employeeId.HasValue)
            {
                query = query.Where(ec => ec.EmployeeId == employeeId.Value);
            }

            // Apply job filter
            if (jobId.HasValue)
            {
                query = query.Where(ec => ec.JobId == jobId.Value);
            }

            // Apply contract type filter
            if (!string.IsNullOrWhiteSpace(contractType))
            {
                if (Enum.TryParse<ContractType>(contractType, true, out var contractTypeEnum))
                {
                    query = query.Where(ec => ec.ContractType == contractTypeEnum);
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

            return new PagedResult<EmployeeContract>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<EmployeeContract> ApplyOrdering(IQueryable<EmployeeContract> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "StartDate";

            query = orderBy.ToLower() switch
            {
                "startdate" => isDescending ? query.OrderByDescending(ec => ec.StartDate) : query.OrderBy(ec => ec.StartDate),
                "enddate" => isDescending ? query.OrderByDescending(ec => ec.EndDate) : query.OrderBy(ec => ec.EndDate),
                "salary" => isDescending ? query.OrderByDescending(ec => ec.Salary) : query.OrderBy(ec => ec.Salary),
                "contracttype" => isDescending ? query.OrderByDescending(ec => ec.ContractType) : query.OrderBy(ec => ec.ContractType),
                _ => isDescending ? query.OrderByDescending(ec => ec.StartDate) : query.OrderBy(ec => ec.StartDate)
            };

            return query;
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