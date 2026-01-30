using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using SharedKernel.Multitenancy;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOfferById
{
    public class GetStorefrontOfferByIdQueryHandler : IRequestHandler<GetStorefrontOfferByIdQueryRequest, GetStorefrontOfferByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetStorefrontOfferByIdQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
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
            
            // Fetch published active products
            var products = await productRepo.GetAllAsync(
                p => productIds.Contains(p.Id) && p.IsPublished && p.IsAvailable);

            var productDtos = products.Select(p => new StorefrontOfferProductDto
                {
                    Id = p.Id,
                    Name = p.NameSnapshot,
                    ImageUrl = p.ImageUrlSnapshot,
                    OriginalPrice = p.Price // Or SalePrice logic if applicable
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
