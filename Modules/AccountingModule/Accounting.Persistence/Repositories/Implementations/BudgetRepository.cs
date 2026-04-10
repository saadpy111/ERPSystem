using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
    {
        public BudgetRepository(AccountingDbContext context) : base(context)
        {
        }
    }

    public class BudgetLineRepository : GenericRepository<BudgetLine>, IBudgetLineRepository
    {
        public BudgetLineRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
