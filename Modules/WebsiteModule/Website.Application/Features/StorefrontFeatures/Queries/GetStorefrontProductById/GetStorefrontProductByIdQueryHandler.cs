using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryHandler : IRequestHandler<GetStorefrontProductByIdQueryRequest, GetStorefrontProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetStorefrontProductByIdQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetStorefrontProductByIdQueryResponse> Handle(GetStorefrontProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<WebsiteProduct>();

            // Ensure IsPublished
            var product = await repo.GetFirstAsync(
                p => p.Id == request.Id && p.IsPublished,
                asNoTracking: true);

            if (product == null)
            {
                return new GetStorefrontProductByIdQueryResponse { Product = null };
            }

            return new GetStorefrontProductByIdQueryResponse
            {
                Product = new StorefrontProductDetailDto
                {
                    Id = product.Id,
                    Name = product.NameSnapshot,
                    ImageUrl = product.ImageUrlSnapshot,
                    CategoryId = product.CategoryId,
                    CategoryName = product.CategoryNameSnapshot,
                    Price = product.Price, 
                    IsAvailable = product.IsAvailable
                }
            };
        }
    }
}
