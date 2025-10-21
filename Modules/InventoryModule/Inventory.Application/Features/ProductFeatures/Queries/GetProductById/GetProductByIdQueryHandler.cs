using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repositories<Product>()
                                  .GetById(request.Id,
                                  p => p.AttributeValues,
                                  p => p.Images);
            if (product == null) return new GetProductByIdQueryResponse();

            var attributeValues = product.AttributeValues?.Select(av => new ProductAttributeValueDto
            {
                AttributeId = av.AttributeId,
                Value = av.Value
            }).ToList();

            var dto = product.ToDto();

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}