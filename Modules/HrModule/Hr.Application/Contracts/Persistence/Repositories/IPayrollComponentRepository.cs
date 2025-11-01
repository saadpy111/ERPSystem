using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IPayrollComponentRepository
    {
        Task<PayrollComponent> AddAsync(PayrollComponent payrollComponent);
        Task<PayrollComponent?> GetByIdAsync(int id);
        Task<IEnumerable<PayrollComponent>> GetAllAsync();
        Task<IEnumerable<PayrollComponent>> GetByPayrollRecordIdAsync(int payrollRecordId);
        void Update(PayrollComponent payrollComponent);
        void Delete(PayrollComponent payrollComponent);
    }
}
