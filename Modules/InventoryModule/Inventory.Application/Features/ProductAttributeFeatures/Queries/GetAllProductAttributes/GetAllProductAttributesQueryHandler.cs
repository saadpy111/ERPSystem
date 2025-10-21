using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductAttributeFeatures.Queries.GetAllProductAttributes
{
    public class GetAllProductAttributesQueryHandler 
        : IRequestHandler<GetAllProductAttributesQueryRequest, GetAllProductAttributesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductAttributesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllProductAttributesQueryResponse> Handle(GetAllProductAttributesQueryRequest request, CancellationToken cancellationToken)
        {
            var attributes = await _unitOfWork.Repositories<ProductAttribute>().GetAll();
            
           var dtos =  attributes.Select(a => new GetProductAttributeDto
            {
                Id = a.Id,
                Name = a.Name,
                DataType = a.DataType,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToList();
            return new GetAllProductAttributesQueryResponse()
            {
                ProductAttributes = dtos
            };
        }
    }
}