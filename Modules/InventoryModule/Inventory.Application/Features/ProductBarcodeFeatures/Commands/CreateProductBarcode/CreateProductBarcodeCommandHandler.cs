using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.CreateProductBarcode
{
    public class CreateProductBarcodeCommandHandler : IRequestHandler<CreateProductBarcodeCommandRequest, CreateProductBarcodeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductBarcodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductBarcodeCommandResponse> Handle(CreateProductBarcodeCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new ProductBarcode
                {
                    BarcodeValue = request.ProductBarcode.BarcodeValue,
                    Type = request.ProductBarcode.Type,
                    ProductId = request.ProductBarcode.ProductId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repositories<ProductBarcode>().Add(entity);
                await _unitOfWork.CompleteAsync();

                return new CreateProductBarcodeCommandResponse
                {
                    Success = true,
                    ProductBarcode = new GetProductBarcodeDto
                    {
                        Id = entity.Id,
                        BarcodeValue = entity.BarcodeValue,
                        Type = entity.Type,
                        ProductId = entity.ProductId,
                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt
                    }
                };
            }
            catch
            {
                return new CreateProductBarcodeCommandResponse { Success = false };
            }
        }
    }
}