using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
