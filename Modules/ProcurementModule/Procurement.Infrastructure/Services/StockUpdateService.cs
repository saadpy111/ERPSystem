using MediatR;
using SharedKernel.DomainEvents;
using System.Threading.Tasks;

namespace Procurement.Infrastructure.Services
{
    public class StockUpdateService : IStockUpdateService
    {
        private readonly IMediator _mediator;
        
        public StockUpdateService(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task UpdateInventoryAsync(GoodsReceivedEvent goodsReceivedEvent)
        {
            // In a real implementation, this would send a command to the Inventory module
            // For now, we'll just publish the event through MediatR
            await _mediator.Publish(goodsReceivedEvent);
        }
    }
}