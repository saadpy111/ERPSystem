using Accounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface ICashAccountRepository : IGenericRepository<CashAccount>
    {
        Task<CashAccount?> GetWithAccountAsync(int id);
        Task<List<CashAccount>> GetAllWithAccountAsync();
    }
}
