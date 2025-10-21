using Inventory.Application.Dtos.StockMoveDtos;
using MediatR;

namespace Inventory.Application.Features.StockMoveFeatures.Commands.CreateStockMove
{
    public class CreateStockMoveCommandRequest : IRequest<CreateStockMoveCommandResponse>
    {
        public CreateStockMoveDto StockMove { get; set; }
    }
}