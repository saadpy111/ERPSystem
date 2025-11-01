using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IPayrollRecordRepository
    {
        Task<PayrollRecord> AddAsync(PayrollRecord payrollRecord);
        Task<PayrollRecord?> GetByIdAsync(int id);
        Task<IEnumerable<PayrollRecord>> GetAllAsync();
        Task<IEnumerable<PayrollRecord>> GetByEmployeeIdAsync(int employeeId);
        void Update(PayrollRecord payrollRecord);
        void Delete(PayrollRecord payrollRecord);
    }
}
