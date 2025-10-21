using Inventory.Application.Pagination;
using Inventory.Application.Dtos.ProductBarcodeDtos;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetPagedProductBarcodes
{
    public class GetPagedProductBarcodesQueryResponse
    {
        public PagedResult<GetProductBarcodeDto> Result { get; set; }
    }
}