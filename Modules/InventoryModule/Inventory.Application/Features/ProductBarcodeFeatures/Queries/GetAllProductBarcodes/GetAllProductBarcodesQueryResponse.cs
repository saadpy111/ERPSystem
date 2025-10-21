using Inventory.Application.Dtos.ProductBarcodeDtos;
using System.Collections.Generic;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetAllProductBarcodes
{
    public class GetAllProductBarcodesQueryResponse
    {
        public List<GetProductBarcodeDto> ProductBarcodes { get; set; } = new();
    }
}