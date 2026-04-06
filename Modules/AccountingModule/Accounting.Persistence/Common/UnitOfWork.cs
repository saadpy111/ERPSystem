using Accounting.Application.Interfaces.Repositories;
using Accounting.Persistence.Context;
using Accounting.Persistence.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Persistence.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccountingDbContext _context;
        private IAccountRepository? _accountRepository;
        private IJournalEntryRepository? _journalEntryRepository;
        private IPartnerRepository? _partnerRepository;
        private ICurrencyRepository? _currencyRepository;
        private ICurrencyRateRepository? _currencyRateRepository;
        private IVoucherRepository? _voucherRepository;
        private IAccountingMappingRepository? _accountingMappingRepository;
        private ICashAccountRepository? _cashAccountRepository;

        public UnitOfWork(AccountingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAccountRepository Accounts => _accountRepository ??= new AccountRepository(_context);
        public IJournalEntryRepository JournalEntries => _journalEntryRepository ??= new JournalEntryRepository(_context);
        public IPartnerRepository Partners => _partnerRepository ??= new PartnerRepository(_context);
        public ICurrencyRepository Currencies => _currencyRepository ??= new CurrencyRepository(_context);
        public ICurrencyRateRepository CurrencyRates => _currencyRateRepository ??= new CurrencyRateRepository(_context);
        public IVoucherRepository Vouchers => _voucherRepository ??= new VoucherRepository(_context);
        public IAccountingMappingRepository AccountingMappings => _accountingMappingRepository ??= new AccountingMappingRepository(_context);
        public ICashAccountRepository CashAccounts => _cashAccountRepository ??= new CashAccountRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await operation();
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
