using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Domain.Entities;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IPayableRepository : IGenericRepository<Payable>
    {
        Task<IEnumerable<Payable>> GetByPartnerAsync(int partnerId);
        Task<IEnumerable<Payable>> GetOpenItemsAsync();
    }
}
