using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using System.Runtime.CompilerServices;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class UnderReviewStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnderReviewStockMoveHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> HandleMove(StockMove stockMove)
        {
            if (stockMove.SourceLocationId == null || stockMove.DestinationLocationId == null)
                return false;

            var isProductExist = await _unitOfWork.Repositories<Product>().Any(p => p.Id == stockMove.ProductId);
            var isSourceExist = await _unitOfWork.Repositories<Location>().Any(l => l.Id == stockMove.SourceLocationId);
            var isDestExist = await _unitOfWork.Repositories<Location>().Any(l => l.Id == stockMove.DestinationLocationId);

            if (!isProductExist || !isSourceExist || !isDestExist)
                return false;

            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();
            var inventoryQuarantineRepo = _unitOfWork.Repositories<InventoryQuarantine>();

            var sourceQuant = await stockQuantRepo.GetFirst(s => s.ProductId == stockMove.ProductId && s.LocationId == stockMove.SourceLocationId);
            if (sourceQuant == null || sourceQuant.Quantity < stockMove.Quantity)
                return false;

            sourceQuant.Quantity -= stockMove.Quantity;

            await inventoryQuarantineRepo.Add(new InventoryQuarantine()
            {
                ProductId = stockMove.ProductId,
                Quantity = stockMove.Quantity,
                LocationId = stockMove.DestinationLocationId.Value,
                CreatedAt = DateTime.UtcNow,
                Status = Domain.Enums.QuarantineStatus.Pending,
                QuarantineDate = DateTime.UtcNow,
                SourceReference = stockMove.Id.ToString()
            });

            return true;
        }

    }
}