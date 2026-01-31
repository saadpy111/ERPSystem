using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using SharedKernel.Multitenancy;
using SharedKernel.Core.Files;
using System.Linq.Expressions;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOfferById
{
    public class GetStorefrontOfferByIdQueryHandler : IRequestHandler<GetStorefrontOfferByIdQueryRequest, GetStorefrontOfferByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFileUrlResolver _urlResolver;

        public GetStorefrontOfferByIdQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, IFileUrlResolver urlResolver)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _urlResolver = urlResolver;
        }

        public async Task<GetStorefrontOfferByIdQueryResponse> Handle(GetStorefrontOfferByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var now = DateTime.UtcNow;

            // 1. Load offer with OfferProducts (1st level include)
            var offer = await repo.GetFirstAsync(
                o => o.Id == request.Id && o.IsActive && o.StartDate <= now && o.EndDate >= now,
                asNoTracking: true,
                o => o.OfferProducts // Include OfferProducts
            );

            if (offer == null)
            {
                return new GetStorefrontOfferByIdQueryResponse { Offer = null };
            }

            // 2. Load Products for these OfferProducts
            var productIds = offer.OfferProducts.Select(op => op.ProductId).ToList();
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            
            // Fetch published active products with images
            var products = await productRepo.GetAllAsync(
                p => productIds.Contains(p.Id) && p.IsPublished && p.IsAvailable,
                p => p.Images);

            var productDtos = products.Select(p => {
                    var primaryImage = p.Images.FirstOrDefault(i => i.IsPrimary) ?? p.Images.OrderBy(i => i.DisplayOrder).FirstOrDefault();
                    return new StorefrontOfferProductDto
                    {
                        Id = p.Id,
                        Name = p.NameSnapshot,
                        ImageUrl = _urlResolver.Resolve(primaryImage?.ImagePath),
                        OriginalPrice = p.Price 
                    };
                })
                .ToList();

            return new GetStorefrontOfferByIdQueryResponse
            {
                Offer = new StorefrontOfferDetailDto
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description,
                    DiscountType = offer.DiscountType,
                    DiscountValue = offer.DiscountValue,
                    StartDate = offer.StartDate,
                    EndDate = offer.EndDate,
                    Products = productDtos
                }
            };
        }
    }
}
