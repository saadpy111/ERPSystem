using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILoanInstallmentRepository
    {
        Task<LoanInstallment> AddAsync(LoanInstallment loanInstallment);
        Task<LoanInstallment?> GetByIdAsync(int id);
        Task<IEnumerable<LoanInstallment>> GetAllAsync();
        Task<IEnumerable<LoanInstallment>> GetByLoanIdAsync(int loanId);
        void Update(LoanInstallment loanInstallment);
        void Delete(LoanInstallment loanInstallment);
    }
}
