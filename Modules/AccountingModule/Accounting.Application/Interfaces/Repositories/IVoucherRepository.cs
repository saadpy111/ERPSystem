using Accounting.Domain.Entities;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<Voucher?> GetByIdWithLinesAsync(int id);
    }
}
