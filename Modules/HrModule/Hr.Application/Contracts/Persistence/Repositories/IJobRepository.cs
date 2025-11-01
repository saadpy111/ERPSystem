using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IJobRepository
    {
        Task<Job> AddAsync(Job job);
        Task<Job?> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync();
        Task<IEnumerable<Job>> GetByDepartmentIdAsync(int departmentId);
        void Update(Job job);
        void Delete(Job job);
    }
}
