using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILoanRepository
    {
        Task<Loan> AddAsync(Loan loan);
        Task<Loan?> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<IEnumerable<Loan>> GetByEmployeeIdAsync(int employeeId);
        void Update(Loan loan);
        void Delete(Loan loan);
    }
}
