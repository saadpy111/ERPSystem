using Accounting.Domain.Enums;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Interfaces
{
    public interface IAccountingMappingService
    {
        Task<int> GetAccountIdAsync(SourceType sourceType, string key);
    }
}
