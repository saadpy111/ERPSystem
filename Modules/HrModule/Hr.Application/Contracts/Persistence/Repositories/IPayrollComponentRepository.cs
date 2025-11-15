using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IPayrollComponentRepository
    {
        Task<PayrollComponent> AddAsync(PayrollComponent payrollComponent);
        Task<PayrollComponent?> GetByIdAsync(int id);
        Task<IEnumerable<PayrollComponent>> GetAllAsync();
        Task<IEnumerable<PayrollComponent>> GetByPayrollRecordIdAsync(int payrollRecordId);
        Task<PagedResult<PayrollComponent>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? payrollRecordId = null, string? componentType = null, string? orderBy = null, bool isDescending = false);
        void Update(PayrollComponent payrollComponent);
        void Delete(PayrollComponent payrollComponent);
    }
}