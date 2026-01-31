using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using Website.Application.Features.StorefrontFeatures.Services;
using Website.Domain.Entities;
using SharedKernel.Core.Files;
using System.Linq.Expressions;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProducts
{
    public class GetStorefrontProductsQueryHandler : IRequestHandler<GetStorefrontProductsQueryRequest, GetStorefrontProductsQueryResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductPricingService _pricingService;
        private readonly IFileUrlResolver _urlResolver;

        public GetStorefrontProductsQueryHandler(
            IWebsiteProductRepository productRepository, 
            IUnitOfWork unitOfWork,
            IProductPricingService pricingService,
            IFileUrlResolver urlResolver)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _urlResolver = urlResolver;
        }

        public async Task<GetStorefrontProductsQueryResponse> Handle(GetStorefrontProductsQueryRequest request, CancellationToken cancellationToken)
        {
            // 1. Get Paginated Products
            var result = await _productRepository.SearchAsync(
                filter: p =>
                    p.IsPublished && p.IsAvailable &&
                    (request.CategoryId == null || p.CategoryId == request.CategoryId) &&
                    (request.MinPrice == null || p.Price >= request.MinPrice) &&
                    (request.MaxPrice == null || p.Price <= request.MaxPrice) &&
                    (string.IsNullOrEmpty(request.Search) || p.NameSnapshot.Contains(request.Search)),
                page: request.Page,
                pageSize: request.PageSize,
                orderBy: q => q.OrderBy(p => p.DisplayOrder).ThenBy(p => p.NameSnapshot),
                includes: new Expression<Func<WebsiteProduct, object>>[] { p => p.Images }
            );

            // 2. Build Product-to-Offers Lookup
            var productIds = result.Items.Select(p => p.Id).ToList();
            var productOffersLookup = await BuildProductOffersLookup(productIds);

            // 3. Map to DTOs with Pricing Calculation
            var productDtos = new List<StorefrontProductDto>();

            foreach (var product in result.Items)
            {
                var applicableOffers = productOffersLookup.ContainsKey(product.Id) 
                    ? productOffersLookup[product.Id] 
                    : new List<Offer>();

                var pricingResult = _pricingService.CalculateBestPrice(product.Price, applicableOffers);

                var primaryImage = product.Images.FirstOrDefault(i => i.IsPrimary) ?? product.Images.OrderBy(i => i.DisplayOrder).FirstOrDefault();

                productDtos.Add(new StorefrontProductDto
                {
                    Id = product.Id,
                    Name = product.NameSnapshot,
                    ImageUrl = _urlResolver.Resolve(primaryImage?.ImagePath),
                    CategoryName = product.CategoryNameSnapshot,
                    OriginalPrice = pricingResult.OriginalPrice,
                    FinalPrice = pricingResult.FinalPrice,
                    DiscountAmount = pricingResult.DiscountAmount,
                    AppliedOfferName = pricingResult.AppliedOfferName,
                    HasOffer = pricingResult.HasOffer,
                    IsAvailable = product.IsAvailable
                });
            }

            var dtoResult = new PagedResult<StorefrontProductDto>
            {
                Items = productDtos,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

            return new GetStorefrontProductsQueryResponse { Result = dtoResult };
        }

        /// <summary>
        /// Efficiently fetch active offers for the visible products.
        /// </summary>
        private async Task<Dictionary<Guid, List<Offer>>> BuildProductOffersLookup(List<Guid> productIds)
        {
            var offerRepo = _unitOfWork.Repository<Offer>();
            var now = DateTime.UtcNow;

            var offers = await offerRepo.GetAllAsync(
                o => o.IsActive && o.StartDate <= now && o.EndDate >= now,
                o => o.OfferProducts);

            return offers
                .SelectMany(o => o.OfferProducts.Select(op => new { op.ProductId, Offer = o }))
                .Where(x => productIds.Contains(x.ProductId))
                .GroupBy(x => x.ProductId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Offer).ToList()
                );
        }
    }
}
