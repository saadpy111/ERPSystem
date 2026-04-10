using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<Voucher?> GetByIdWithLinesAsync(int id)
        {
            return await _dbSet
                .Include(v => v.Lines)
                .Include(v => v.Partner)
                .Include(v => v.Currency)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
