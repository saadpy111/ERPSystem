using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers
{
    public interface IStockMoveHandler
    {
        Task<bool> HandleMove(StockMove stockMove);
    }
}
