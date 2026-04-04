using System;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IJournalEntryRepository JournalEntries { get; }
        IPartnerRepository Partners { get; }
        ICurrencyRepository Currencies { get; }
        IVoucherRepository Vouchers { get; }

        Task<int> SaveChangesAsync();
    }
}
