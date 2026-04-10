using System;
using System.Threading.Tasks;
using System.Threading;
using Accounting.Domain.Entities;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IJournalEntryRepository JournalEntries { get; }
        IPartnerRepository Partners { get; }
        ICurrencyRepository Currencies { get; }
        ICurrencyRateRepository CurrencyRates { get; }
        IVoucherRepository Vouchers { get; }
        IAccountingMappingRepository AccountingMappings { get; }
        ICashAccountRepository CashAccounts { get; }
        IReceivableRepository Receivables { get; }
        IGenericRepository<ReceivablePayment> ReceivablePayments { get; }
        IPayableRepository Payables { get; }
        IGenericRepository<PayablePayment> PayablePayments { get; }
        IGenericRepository<VoucherLine> VoucherLines { get; }

        Task<int> SaveChangesAsync();
        Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
    }
}
