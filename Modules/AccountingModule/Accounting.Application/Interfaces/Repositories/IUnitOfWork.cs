using System;
using System.Threading.Tasks;
using System.Threading;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IJournalEntryRepository JournalEntries { get; }
        IPartnerRepository Partners { get; }
        ICurrencyRepository Currencies { get; }
        IVoucherRepository Vouchers { get; }
        IAccountingMappingRepository AccountingMappings { get; }

        Task<int> SaveChangesAsync();
        Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
    }
}
