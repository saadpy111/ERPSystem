using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers;
using Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Factory
{
    public interface IStockMoveHandlerFactory
    {
        IStockMoveHandler GetHandler(StockMoveType moveType);

    }
}
