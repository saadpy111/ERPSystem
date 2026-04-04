using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.External
{
    public class ExternalSalesInvoice
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ExternalPurchaseInvoice
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ExternalInventoryMove
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
    }

    public interface ISalesRepository
    {
        Task<ExternalSalesInvoice> GetByIdAsync(int id);
    }

    public interface IPurchaseRepository
    {
        Task<ExternalPurchaseInvoice> GetByIdAsync(int id);
    }

    public interface IInventoryRepository
    {
        Task<ExternalInventoryMove> GetByIdAsync(int id);
    }
}
