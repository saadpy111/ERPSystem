using Inventory.Application.Dtos.StockQuantDtos;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetStockQuantById
{
    public class GetStockQuantByIdQueryResponse
    {
        public GetStockQuantDto? StockQuant { get; set; }
    }
}