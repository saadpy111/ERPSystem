using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class FiscalYearRepository : GenericRepository<FiscalYear>, IFiscalYearRepository
    {
        public FiscalYearRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
