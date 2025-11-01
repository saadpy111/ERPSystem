using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> AddAsync(Department department);
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        void Update(Department department);
        void Delete(Department department);
    }
}
