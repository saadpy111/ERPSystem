using Accounting.Application.Interfaces.External;
using System.Threading.Tasks;

namespace Accounting.Persistence.External.Fakes
{
    public class FakeSalesRepository : ISalesRepository
    {
        public Task<ExternalSalesInvoice> GetByIdAsync(int id)
        {
            // Safely fallback when real external modules aren't available yet
            return Task.FromResult<ExternalSalesInvoice>(null!);
        }
    }

    public class FakePurchaseRepository : IPurchaseRepository
    {
        public Task<ExternalPurchaseInvoice> GetByIdAsync(int id)
        {
            // Safely fallback when real external modules aren't available yet
            return Task.FromResult<ExternalPurchaseInvoice>(null!);
        }
    }

    public class FakeInventoryRepository : IInventoryRepository
    {
        public Task<ExternalInventoryMove> GetByIdAsync(int id)
        {
            // Safely fallback when real external modules aren't available yet
            return Task.FromResult<ExternalInventoryMove>(null!);
        }
    }
}
