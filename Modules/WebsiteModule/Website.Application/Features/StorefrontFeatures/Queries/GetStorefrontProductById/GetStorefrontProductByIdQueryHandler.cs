using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using SharedKernel.Core.Files;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById
{
    public class GetStorefrontProductByIdQueryHandler : IRequestHandler<GetStorefrontProductByIdQueryRequest, GetStorefrontProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFileUrlResolver _urlResolver;

        public GetStorefrontProductByIdQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, IFileUrlResolver urlResolver)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _urlResolver = urlResolver;
        }

        public async Task<GetStorefrontProductByIdQueryResponse> Handle(GetStorefrontProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<WebsiteProduct>();

            // Ensure IsPublished
            var product = await repo.GetFirstAsync(
                p => p.Id == request.Id && p.IsPublished,
                asNoTracking: true,
                p => p.Images);

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
                    Images = product.Images.Select(img => new StorefrontProductImageDto
                    {
                        Id = img.Id,
                        ImageUrl = _urlResolver.Resolve(img.ImagePath) ?? string.Empty,
                        AltText = img.AltText,
                        IsPrimary = img.IsPrimary,
                        DisplayOrder = img.DisplayOrder
                    }).OrderBy(i => i.DisplayOrder).ToList(),
                    CategoryId = product.CategoryId,
                    CategoryName = product.CategoryNameSnapshot,
                    Price = product.Price, 
                    IsAvailable = product.IsAvailable
                }
            };
        }
    }
}
