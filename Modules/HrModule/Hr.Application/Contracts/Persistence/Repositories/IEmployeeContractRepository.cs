using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IEmployeeContractRepository
    {
        Task<EmployeeContract> AddAsync(EmployeeContract employeeContract);
        Task<EmployeeContract?> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeContract>> GetAllAsync();
        Task<PagedResult<EmployeeContract>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, int? jobId = null, string? contractType = null, string? orderBy = null, bool isDescending = false);
        void Update(EmployeeContract employeeContract);
        void Delete(EmployeeContract employeeContract);
        Task<EmployeeContract?> GetContractByEmployeeIdAsync(int id);
        Task<IEnumerable<EmployeeContract>> GetContractsByEmployeeIdAsync(int employeeId);
    }
}