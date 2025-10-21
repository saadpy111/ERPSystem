using Inventory.Application.Dtos.ProductBarcodeDtos;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.CreateProductBarcode
{
    public class CreateProductBarcodeCommandResponse
    {
        public bool Success { get; set; }
        public GetProductBarcodeDto? ProductBarcode { get; set; }
    }
}