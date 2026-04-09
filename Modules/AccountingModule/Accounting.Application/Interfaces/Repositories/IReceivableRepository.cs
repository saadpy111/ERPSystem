using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Domain.Entities;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IReceivableRepository : IGenericRepository<Receivable>
    {
        Task<IEnumerable<Receivable>> GetByPartnerAsync(int partnerId);
        Task<IEnumerable<Receivable>> GetOpenItemsAsync();
    }
}
