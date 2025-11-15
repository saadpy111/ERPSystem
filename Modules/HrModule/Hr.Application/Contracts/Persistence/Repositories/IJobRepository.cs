using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IJobRepository
    {
        Task<Job> AddAsync(Job job);
        Task<Job?> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync();
        Task<IEnumerable<Job>> GetByDepartmentIdAsync(int departmentId);
        Task<PagedResult<Job>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? departmentId = null, string? status = null, string? workType = null, string? orderBy = null, bool isDescending = false);
        void Update(Job job);
        void Delete(Job job);
    }
}