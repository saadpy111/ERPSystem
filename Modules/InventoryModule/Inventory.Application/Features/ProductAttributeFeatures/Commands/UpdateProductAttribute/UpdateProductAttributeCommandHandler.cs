using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Net.Http.Headers;

namespace Inventory.Application.Features.ProductAttributeFeatures.Commands.UpdateProductAttribute
{
    public class UpdateProductAttributeCommandHandler 
        : IRequestHandler<UpdateProductAttributeCommandRequest, UpdateProductAttributeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductAttributeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductAttributeCommandResponse> Handle(UpdateProductAttributeCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<ProductAttribute>();
                var entity = await repo.GetById(request.ProductAttribute.Id);

                if (entity == null) return  new UpdateProductAttributeCommandResponse()
                {
                     Success = false,
                  
                };

                entity.Name = request.ProductAttribute.Name;
                entity.DataType = request.ProductAttribute.DataType;
                entity.UpdatedAt = DateTime.UtcNow;
                repo.Update(entity);
                await _unitOfWork.CompleteAsync();

                var dto = new GetProductAttributeDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    DataType = entity.DataType,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                };

                return new UpdateProductAttributeCommandResponse()
                {
                    Success = true,
                    ProductAttribute = dto
                };
            }
            catch
            {
                return new UpdateProductAttributeCommandResponse()
                {
                    Success = false
                };
            }
            
        }
    }
}