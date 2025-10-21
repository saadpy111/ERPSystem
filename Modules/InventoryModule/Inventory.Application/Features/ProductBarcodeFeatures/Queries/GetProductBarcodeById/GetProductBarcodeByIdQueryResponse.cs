using Inventory.Application.Dtos.ProductBarcodeDtos;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetProductBarcodeById
{
    public class GetProductBarcodeByIdQueryResponse
    {
        public GetProductBarcodeDto? ProductBarcode { get; set; }
    }
}