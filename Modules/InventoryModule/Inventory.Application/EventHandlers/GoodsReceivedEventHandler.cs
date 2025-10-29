using MediatR;
using Inventory.Application.Features.StockMoveFeatures.Commands.CreateStockMove;
using Inventory.Application.Dtos.StockMoveDtos;
using Inventory.Domain.Enums;
using SharedKernel.DomainEvents;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.EventHandlers
{
    public class GoodsReceivedEventHandler : INotificationHandler<GoodsReceivedEvent>
    {
        private readonly IMediator _mediator;

        public GoodsReceivedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(GoodsReceivedEvent notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.Items)
            {
                var command = new CreateStockMoveCommandRequest
                {
                    StockMove = new CreateStockMoveDto
                    {
                        ProductId = item.ProductId,
                        Quantity = (int)item.Quantity,
                        MoveType = StockMoveType.Purchase,
                        Reference = $"GoodsReceipt-{notification.GoodsReceiptId}",
                        DestinationLocationId = notification.LocationId
                    }
                };

                await _mediator.Send(command, cancellationToken);
            }
        }
    }
}
