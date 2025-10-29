using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public class PurchaseStockMoveHandler : IStockMoveHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseStockMoveHandler(IUnitOfWork unitOfWork)
        {
               _unitOfWork = unitOfWork;
        }
        public async Task<bool> HandleMove(StockMove stockMove)
        {
            // check values
            if (stockMove.DestinationLocationId == null)
                return false;

            // check product exist or not 
            var productRepo = _unitOfWork.Repositories<Product>();

            var productExist = await productRepo.Any(p => p.Id == stockMove.ProductId);

            if (!productExist)
                return false;

            // check location exist or not

            var locationRepo = _unitOfWork.Repositories<Location>();



            var locationExist = await locationRepo.Any(l => l.Id == stockMove.DestinationLocationId);

            if (!locationExist)
                return false;




            Expression<Func<StockQuant, bool>> filter =
                      s =>s.ProductId==stockMove.ProductId && s.LocationId == stockMove.DestinationLocationId;

            var stockQuantRepo = _unitOfWork.Repositories<StockQuant>();
            var stockquant = await stockQuantRepo.GetFirst(filter);

            if(stockquant == null)
            {
                       await  stockQuantRepo.Add(new StockQuant()
                       {
                            LocationId = stockMove.DestinationLocationId.Value,
                             CreatedAt = DateTime.UtcNow,
                              ReservedQuantity = 0,
                               Quantity = stockMove.Quantity,
                                ProductId = stockMove.ProductId
                       });
            }

            else
            {
                stockquant.Quantity += stockMove.Quantity;
                 stockQuantRepo.Update(stockquant);

            }

            return true;                    
        }
    }
}
