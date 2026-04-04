using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
