using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockMoveDtos;
using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Factory;
using Inventory.Domain.Entities;
using Events.InventoryEvents;
using MediatR;

namespace Inventory.Application.Features.StockMoveFeatures.Commands.CreateStockMove
{
    public class CreateStockMoveCommandHandler
        : IRequestHandler<CreateStockMoveCommandRequest, CreateStockMoveCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IStockMoveHandlerFactory _handlerFactory;

        public CreateStockMoveCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IStockMoveHandlerFactory handlerFactory)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _handlerFactory = handlerFactory;
        }

        public async Task<CreateStockMoveCommandResponse> Handle(CreateStockMoveCommandRequest request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var entity = new StockMove
                {
                    Quantity = request.StockMove.Quantity,
                    MoveDate = DateTime.UtcNow,
                    Reference = request.StockMove.Reference,
                    MoveType = request.StockMove.MoveType,
                    ProductId = request.StockMove.ProductId,
                    SourceLocationId = request.StockMove.SourceLocationId,
                    DestinationLocationId = request.StockMove.DestinationLocationId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repositories<StockMove>().Add(entity);

                var handler = _handlerFactory.GetHandler(request.StockMove.MoveType);
                var handled = await handler.HandleMove(entity);

                if (!handled)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new CreateStockMoveCommandResponse { Success = false };
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Publish domain event after successful commit
                await _mediator.Publish(new StockMoveCreatedEvent
                {
                    StockMoveId = entity.Id,
                    ProductId = entity.ProductId,
                    Quantity = entity.Quantity,
                    MoveType = entity.MoveType.ToString(),
                    Reference = entity.Reference,
                    SourceLocationId = entity.SourceLocationId,
                    DestinationLocationId = entity.DestinationLocationId,
                    MoveDate = entity.MoveDate
                }, cancellationToken);

                return new CreateStockMoveCommandResponse { Success = true };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new CreateStockMoveCommandResponse { Success = false };
            }
        }
    }
}
