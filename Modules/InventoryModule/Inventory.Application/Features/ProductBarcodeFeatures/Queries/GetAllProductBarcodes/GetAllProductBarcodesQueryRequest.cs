using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetAllProductBarcodes
{
    public class GetAllProductBarcodesQueryRequest : IRequest<GetAllProductBarcodesQueryResponse>
    {
        public string? BarcodeValue { get; set; }
    }
}