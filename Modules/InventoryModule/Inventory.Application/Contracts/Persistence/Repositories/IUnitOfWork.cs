using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Contracts.Persistence.Repositories
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepository<T> Repositories<T>() where T:class;
        Task<int> CompleteAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        void PrintEntityStates();

    }
}
