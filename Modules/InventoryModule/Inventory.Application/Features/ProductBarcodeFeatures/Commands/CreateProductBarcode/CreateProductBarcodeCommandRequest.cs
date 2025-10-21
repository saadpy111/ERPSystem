using Inventory.Application.Dtos.ProductBarcodeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.CreateProductBarcode
{
    public class CreateProductBarcodeCommandRequest : IRequest<CreateProductBarcodeCommandResponse>
    {
        public CreateProductBarcodeDto ProductBarcode { get; set; }
    }
}