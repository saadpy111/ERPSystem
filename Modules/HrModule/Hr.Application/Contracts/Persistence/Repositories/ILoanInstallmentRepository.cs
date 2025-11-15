using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILoanInstallmentRepository
    {
        Task<LoanInstallment> AddAsync(LoanInstallment loanInstallment);
        Task<LoanInstallment?> GetByIdAsync(int id);
        Task<IEnumerable<LoanInstallment>> GetAllAsync();
        Task<IEnumerable<LoanInstallment>> GetAllPendingAsync();
        Task<IEnumerable<LoanInstallment>> GetByLoanIdAsync(int loanId);
        Task<PagedResult<LoanInstallment>> GetPagedAsync(int pageNumber, int pageSize, int? loanId = null, string? status = null, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        void Update(LoanInstallment loanInstallment);
        void Delete(LoanInstallment loanInstallment);
    }
}