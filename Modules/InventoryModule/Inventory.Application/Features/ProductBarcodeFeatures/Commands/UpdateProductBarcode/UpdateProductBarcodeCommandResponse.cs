using Inventory.Application.Dtos.ProductBarcodeDtos;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.UpdateProductBarcode
{
    public class UpdateProductBarcodeCommandResponse
    {
        public bool Success { get; set; }
        public GetProductBarcodeDto? ProductBarcode { get; set; }
    }
}