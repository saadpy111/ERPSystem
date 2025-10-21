using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetProductBarcodeById
{
    public class GetProductBarcodeByIdQueryRequest : IRequest<GetProductBarcodeByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}