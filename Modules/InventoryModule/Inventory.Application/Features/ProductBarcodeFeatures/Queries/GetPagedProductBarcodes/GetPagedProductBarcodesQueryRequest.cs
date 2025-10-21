using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetPagedProductBarcodes
{
    public class GetPagedProductBarcodesQueryRequest : IRequest<GetPagedProductBarcodesQueryResponse>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}