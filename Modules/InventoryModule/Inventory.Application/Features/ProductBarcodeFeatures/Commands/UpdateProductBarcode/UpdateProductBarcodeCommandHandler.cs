using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Commands.UpdateProductBarcode
{
    public class UpdateProductBarcodeCommandHandler : IRequestHandler<UpdateProductBarcodeCommandRequest, UpdateProductBarcodeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductBarcodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductBarcodeCommandResponse> Handle(UpdateProductBarcodeCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<ProductBarcode>();
                var entity = await repo.GetById(request.ProductBarcode.Id);
                if (entity == null)
                    return new UpdateProductBarcodeCommandResponse { Success = false };

                entity.BarcodeValue = request.ProductBarcode.BarcodeValue;
                entity.Type = request.ProductBarcode.Type;
                entity.ProductId = request.ProductBarcode.ProductId;
                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
                await _unitOfWork.CompleteAsync();

                return new UpdateProductBarcodeCommandResponse
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
                return new UpdateProductBarcodeCommandResponse { Success = false };

            }

        }
    }
}