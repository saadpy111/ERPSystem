using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ISalaryStructureComponentRepository
    {
        Task<SalaryStructureComponent> AddAsync(SalaryStructureComponent component);
        Task<SalaryStructureComponent?> GetByIdAsync(int id);
        Task<IEnumerable<SalaryStructureComponent>> GetAllAsync();
        Task<IEnumerable<SalaryStructureComponent>> GetBySalaryStructureIdAsync(int salaryStructureId);
        void Update(SalaryStructureComponent component);
        void Delete(SalaryStructureComponent component);
    }
}