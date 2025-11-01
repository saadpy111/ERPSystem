using Hr.Application.Contracts.Persistence;
using Hr.Persistence.Context;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HrDbContext _context;

        public UnitOfWork(HrDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
