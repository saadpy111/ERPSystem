using Accounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<Account>> GetAccountTreeAsync();
        Task<IEnumerable<Account>> GetLeafAccountsAsync();
    }
}
