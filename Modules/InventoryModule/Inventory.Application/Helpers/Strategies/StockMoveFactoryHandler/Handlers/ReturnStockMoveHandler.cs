using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class ReturnStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnStockMoveHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleMove(StockMove stockMove)
        {
            // check values
            if ( stockMove.DestinationLocationId == null)
                return false;

            // check product exist
            var productRepo = _unitOfWork.Repositories<Product>();
            var productExist = await productRepo.Any(p => p.Id == stockMove.ProductId);
            if (!productExist)
                return false;

            // check location exist
            var locationRepo = _unitOfWork.Repositories<Location>();
            var locationExist = await locationRepo.Any(l => l.Id == stockMove.DestinationLocationId.Value);
            if (!locationExist)
                return false;

            // check stock quant
            Expression<Func<StockQuant, bool>> filter =
                s => s.ProductId == stockMove.ProductId && s.LocationId == stockMove.DestinationLocationId;

            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();
            var stockQuant = await stockQuantRepo.GetFirst(filter);

            // if not exist, create new
            if (stockQuant == null)
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
                // just increase quantity for returned stock
                stockQuant.Quantity += stockMove.Quantity;
                stockQuantRepo.Update(stockQuant);
            }

            return true;
        }
    }
}
