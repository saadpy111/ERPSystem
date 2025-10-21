using Inventory.Application.Dtos.StockQuantDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetAllStockQuants
{
    public class GetAllStockQuantsQueryResponse
    {
        public List<GetStockQuantDto> StockQuants { get; set; } = new();
    }
}