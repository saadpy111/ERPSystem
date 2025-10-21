using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class AdjustmentStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdjustmentStockMoveHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleMove(StockMove stockMove)
        {
            if (stockMove.DestinationLocationId == null)
                return false;

            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();
            var stockAdjustmentRepo = _unitOfWork.Repositories<StockAdjustment>();
            var locationRepo = _unitOfWork.Repositories<Location>();

            var location = await locationRepo.GetFirst(l => l.Id == stockMove.DestinationLocationId);
            if (location == null || location.WarehouseId == Guid.Empty)
                return false;

            var warehouseId = location.WarehouseId;

            var existingQuant = await stockQuantRepo.GetFirst(
                q => q.ProductId == stockMove.ProductId && q.LocationId == stockMove.DestinationLocationId
            );

            int actualQty = stockMove.Quantity;

            if (existingQuant != null)
            {
                int expectedQty = existingQuant.Quantity;

                var adjustment = new StockAdjustment
                {
                    ExpectedQuantity = expectedQty,
                    ActualQuantity = actualQty,
                    Date = DateTime.UtcNow,
                    ProductId = stockMove.ProductId,
                    WarehouseId = warehouseId,
                    UserId = Guid.NewGuid()
                };

                await stockAdjustmentRepo.Add(adjustment);

                existingQuant.Quantity = actualQty;
                stockQuantRepo.Update(existingQuant);
            }
            else
            {
                var newQuant = new StockQuant
                {
                    ProductId = stockMove.ProductId,
                    LocationId = stockMove.DestinationLocationId.Value,
                    Quantity = actualQty
                };
                await stockQuantRepo.Add(newQuant);

                var adjustment = new StockAdjustment
                {
                    ExpectedQuantity = 0,
                    ActualQuantity = actualQty,
                    Date = DateTime.UtcNow,
                    ProductId = stockMove.ProductId,
                    WarehouseId = warehouseId,
                    UserId = Guid.NewGuid()
                };
                await stockAdjustmentRepo.Add(adjustment);
            }

            return true;
        }
    }
}
