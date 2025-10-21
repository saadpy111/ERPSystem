using Inventory.Application.Dtos.ProductBarcodeDtos;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.UpdateProductBarcode
{
    public class UpdateProductBarcodeCommandRequest : IRequest<UpdateProductBarcodeCommandResponse>
    {
        public UpdateProductBarcodeDto ProductBarcode { get; set; }
    }
}