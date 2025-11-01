using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IproductRepo _productRepo;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork , IproductRepo productRepo)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await  _productRepo
                                  .GetProductDetailsById(request.Id);
            if (product == null) return new GetProductByIdQueryResponse();

  
            var dto = product.ToDto();

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}