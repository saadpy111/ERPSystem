using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class SalesStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesStockMoveHandler(IUnitOfWork unitOfWork)
        {
               _unitOfWork = unitOfWork;
        }
        async Task<bool> IStockMoveHandler.HandleMove(StockMove stockMove)
        {
            // check values
            if (stockMove.SourceLocationId == null)
                return false;

            // check product exist or not 
            var productRepo = _unitOfWork.Repositories<Product>();

            var productExist = await productRepo.Any( p => p.Id ==  stockMove.ProductId);

            if (!productExist)
                return false;

            // check location exist or not

            var locationRepo = _unitOfWork.Repositories<Location>();

            var locationExist = await locationRepo.Any( l=>l.Id == stockMove.SourceLocationId.Value);

            if (!locationExist)
                return false;




            Expression<Func<StockQuant, bool>> filter =
                      s => s.ProductId == stockMove.ProductId && s.LocationId == stockMove.SourceLocationId;

            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();
            var stockquant = await stockQuantRepo.GetFirst(filter);

            if (stockquant == null || stockquant.Quantity < stockMove.Quantity)
                return false;

            stockquant.Quantity -= stockMove.Quantity;


            return true;
            
        }
    }
}
