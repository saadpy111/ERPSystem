using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILoanRepository
    {
        Task<Loan> AddAsync(Loan loan);
        Task<Loan?> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<IEnumerable<Loan>> GetByEmployeeIdAsync(int employeeId);
        Task<PagedResult<Loan>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, string? status = null, string? orderBy = null, bool isDescending = false);
        void Update(Loan loan);
        void Delete(Loan loan);
    }
}