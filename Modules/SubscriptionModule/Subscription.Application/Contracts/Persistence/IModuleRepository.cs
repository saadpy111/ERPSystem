using Subscription.Domain.Entities;

namespace Subscription.Application.Contracts.Persistence
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(string id);
        Task<Module?> GetByCodeAsync(string code);
        Task<List<Module>> GetAllActiveAsync();
    }
}
