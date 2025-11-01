using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
