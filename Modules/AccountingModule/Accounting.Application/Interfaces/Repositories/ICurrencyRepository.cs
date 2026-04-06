using Accounting.Domain.Entities;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        Task<Currency?> GetBaseCurrencyAsync();
        Task<bool> IsCodeUniqueAsync(string code);
        Task<Currency?> GetWithRatesAsync(int id);
    }
}
