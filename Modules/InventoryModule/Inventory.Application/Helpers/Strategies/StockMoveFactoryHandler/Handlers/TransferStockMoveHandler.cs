using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class TransferStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferStockMoveHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleMove(StockMove stockMove)
        {
            // check values
            if (stockMove.SourceLocationId == null || stockMove.DestinationLocationId == null)
                return false;

            var productRepo = _unitOfWork.Repositories<Product>();
            var locationRepo = _unitOfWork.Repositories<Location>();
            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();

            // check product exist
            var productExist = await productRepo.Any(p => p.Id == stockMove.ProductId);
            if (!productExist)
                return false;

            // check both locations exist
            var sourceExist = await locationRepo.Any(l => l.Id == stockMove.SourceLocationId.Value);
            var destExist = await locationRepo.Any(l => l.Id == stockMove.DestinationLocationId.Value);
            if (!sourceExist || !destExist)
                return false;

            // get source stock quant
            Expression<Func<StockQuant, bool>> sourceFilter =
                s => s.ProductId == stockMove.ProductId && s.LocationId == stockMove.SourceLocationId;

            var sourceStock = await stockQuantRepo.GetFirst(sourceFilter);
            if (sourceStock == null || sourceStock.Quantity < stockMove.Quantity)
                return false; // not enough stock to transfer

            // deduct from source
            sourceStock.Quantity -= stockMove.Quantity;
            stockQuantRepo.Update(sourceStock);

            // get or create destination stock quant
            Expression<Func<StockQuant, bool>> destFilter =
                s => s.ProductId == stockMove.ProductId && s.LocationId == stockMove.DestinationLocationId;

            var destStock = await stockQuantRepo.GetFirst(destFilter);

            if (destStock == null)
            {
                await stockQuantRepo.Add(new StockQuant
                {
                    ProductId = stockMove.ProductId,
                    LocationId = stockMove.DestinationLocationId.Value,
                    Quantity = stockMove.Quantity,
                    ReservedQuantity = 0,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                destStock.Quantity += stockMove.Quantity;
                stockQuantRepo.Update(destStock);
            }

            return true;
        }
    }
}
