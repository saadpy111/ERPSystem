using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.CreateProductAttribute
{
    public class CreateProductAttributeCommandHandler 
        : IRequestHandler<CreateProductAttributeCommandRequest, CreateProductAttributeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductAttributeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductAttributeCommandResponse> Handle(CreateProductAttributeCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new ProductAttribute
                {
                    Name = request.ProductAttribute.Name,
                    DataType = request.ProductAttribute.DataType,
                     CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repositories<ProductAttribute>().Add(entity);
                await _unitOfWork.CompleteAsync();

                var dto = new GetProductAttributeDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    DataType = entity.DataType,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };
                return new CreateProductAttributeCommandResponse()
                {
                    Success = true,
                    ProductAttribute = dto
                };

            }
            catch
            {
                return new CreateProductAttributeCommandResponse()
                {
                    Success = false
                };
            }
        }
    }
}   