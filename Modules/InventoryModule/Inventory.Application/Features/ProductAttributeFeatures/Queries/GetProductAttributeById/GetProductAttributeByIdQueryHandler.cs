using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Queries.GetProductAttributeById
{
    public class GetProductAttributeByIdQueryHandler 
        : IRequestHandler<GetProductAttributeByIdQueryRequest, GetProductAttributeByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductAttributeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductAttributeByIdQueryResponse> Handle(GetProductAttributeByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<ProductAttribute>().GetById(request.Id);
            
            if (entity == null) return null;

            var dto =  new GetProductAttributeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DataType = entity.DataType,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return new GetProductAttributeByIdQueryResponse()
            {
                ProductAttribute = dto
            };
        }
    }
}