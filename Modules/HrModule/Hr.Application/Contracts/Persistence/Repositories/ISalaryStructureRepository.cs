using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ISalaryStructureRepository
    {
        Task<SalaryStructure> AddAsync(SalaryStructure salaryStructure);
        Task<SalaryStructure?> GetByIdAsync(int id);
        Task<IEnumerable<SalaryStructure>> GetAllAsync();
        Task<PagedResult<SalaryStructure>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        Task<IEnumerable<SalaryStructure>> GetActiveAsync();
        void Update(SalaryStructure salaryStructure);
        void Delete(SalaryStructure salaryStructure);
    }
}