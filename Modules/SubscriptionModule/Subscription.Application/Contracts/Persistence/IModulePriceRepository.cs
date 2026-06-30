using SharedKernel.Enums;
using Subscription.Domain.Entities;

namespace Subscription.Application.Contracts.Persistence
{
    public interface IModulePriceRepository
    {
        Task<List<ModulePrice>> GetByModuleIdAsync(string moduleId);
        Task<ModulePrice?> GetActivePriceAsync(string moduleId, string currencyCode, BillingInterval interval);
    }
}
