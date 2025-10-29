using SharedKernel.DomainEvents;
using System.Threading.Tasks;

namespace Procurement.Infrastructure.Services
{
    public interface IStockUpdateService
    {
        Task UpdateInventoryAsync(GoodsReceivedEvent goodsReceivedEvent);
    }
}