using Accounting.Application.Interfaces.Repositories;
using Accounting.Persistence.Context;
using Accounting.Persistence.Repositories.Implementations;
using System;
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
        private IVoucherRepository? _voucherRepository;

        public UnitOfWork(AccountingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAccountRepository Accounts => _accountRepository ??= new AccountRepository(_context);
        public IJournalEntryRepository JournalEntries => _journalEntryRepository ??= new JournalEntryRepository(_context);
        public IPartnerRepository Partners => _partnerRepository ??= new PartnerRepository(_context);
        public ICurrencyRepository Currencies => _currencyRepository ??= new CurrencyRepository(_context);
        public IVoucherRepository Vouchers => _voucherRepository ??= new VoucherRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
