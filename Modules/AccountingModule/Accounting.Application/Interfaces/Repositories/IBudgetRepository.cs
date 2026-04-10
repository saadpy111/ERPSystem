using Accounting.Domain.Entities;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
    }

    public interface IBudgetLineRepository : IGenericRepository<BudgetLine>
    {
    }
}
