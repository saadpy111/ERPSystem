using Accounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<IEnumerable<Partner>> GetCustomersAsync();
        Task<IEnumerable<Partner>> GetVendorsAsync();
    }
}
