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
        void Update(EmployeeContract employeeContract);
        void Delete(EmployeeContract employeeContract);
        Task<EmployeeContract?> GetContractByEmployeeIdAsync(int id);
    }
}