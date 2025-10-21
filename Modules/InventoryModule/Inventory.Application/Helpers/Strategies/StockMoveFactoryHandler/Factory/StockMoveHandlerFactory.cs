using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Factory;
using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers;
using Inventory.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

public class StockMoveHandlerFactory : IStockMoveHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public StockMoveHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStockMoveHandler GetHandler(StockMoveType moveType)
    {
        return moveType switch
        {
            StockMoveType.Transfer => _serviceProvider.GetRequiredService<TransferStockMoveHandler>(),
            StockMoveType.Sale => _serviceProvider.GetRequiredService<SalesStockMoveHandler>(),
            StockMoveType.Return => _serviceProvider.GetRequiredService<ReturnStockMoveHandler>(),
            StockMoveType.Adjustment => _serviceProvider.GetRequiredService<AdjustmentStockMoveHandler>(),
            StockMoveType.Purchase => _serviceProvider.GetRequiredService<PurchaseStockMoveHandler>(),
            StockMoveType.UnderReview => _serviceProvider.GetRequiredService<UnderReviewStockMoveHandler>(),
            _ => throw new ArgumentException("Invalid stock quant operation type.")
        };
    }
}
