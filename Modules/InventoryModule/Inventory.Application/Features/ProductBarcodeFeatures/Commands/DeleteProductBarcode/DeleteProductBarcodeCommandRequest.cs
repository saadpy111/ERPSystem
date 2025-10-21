using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.DeleteProductBarcode
{
    public class DeleteProductBarcodeCommandRequest : IRequest<DeleteProductBarcodeCommandResponse>
    {
        public Guid Id { get; set; }
    }
}