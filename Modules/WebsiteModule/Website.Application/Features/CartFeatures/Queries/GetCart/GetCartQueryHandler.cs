using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Services;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Queries.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQueryRequest, GetCartQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IPricingService _pricingService;

        public GetCartQueryHandler(
            IUnitOfWork unitOfWork, 
            ITenantProvider tenantProvider,
            IPricingService pricingService)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _pricingService = pricingService;
        }

        public async Task<GetCartQueryResponse> Handle(GetCartQueryRequest request, CancellationToken cancellationToken)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                asNoTracking: true,
                c => c.Items);

            if (cart == null)
            {
                // Return empty cart
                return new GetCartQueryResponse
                {
                    Cart = new CartDto
                    {
                        UserId = request.UserId,
                        Items = new List<CartItemDto>(),
                        Subtotal = 0,
                        EstimatedDiscountTotal = 0,
                        EstimatedTotal = 0
                    }
                };
            }

            // Load products for items
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = await productRepo.GetAllAsync(p => productIds.Contains(p.Id));

            // OPTIMIZATION: Build product-to-offers lookup dictionary once
            var productOffersLookup = await BuildProductOffersLookup(productIds);

            // Calculate estimated pricing with offers
            var cartItems = new List<CartItemDto>();
            decimal totalDiscount = 0;

            foreach (var item in cart.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                var itemSubtotal = item.Quantity * item.UnitPrice;

                // Get pre-filtered offers for this specific product (O(1) lookup)
                var applicableOffers = productOffersLookup.ContainsKey(item.ProductId)
                    ? productOffersLookup[item.ProductId]
                    : new List<Offer>();

                // Calculate best offer using only relevant offers
                var offerResult = _pricingService.CalculateBestOffer(
                    item.UnitPrice, 
                    item.Quantity, 
                    applicableOffers);

                totalDiscount += offerResult.DiscountAmount;

                cartItems.Add(new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product?.NameSnapshot ?? "Unknown",
                    ProductImageUrl = product?.ImageUrlSnapshot,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = itemSubtotal,
                    IsAvailable = product?.IsAvailable ?? false,
                    EstimatedDiscount = offerResult.DiscountAmount,
                    EstimatedFinalPrice = offerResult.FinalPrice,
                    AppliedOfferName = offerResult.AppliedOfferName
                });
            }

            var subtotal = cart.Items.Sum(i => i.Quantity * i.UnitPrice);

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cartItems,
                Subtotal = subtotal,
                EstimatedDiscountTotal = totalDiscount,
                EstimatedTotal = subtotal - totalDiscount
            };

            return new GetCartQueryResponse { Cart = cartDto };
        }

        /// <summary>
        /// Build a lookup dictionary mapping ProductId to applicable active offers.
        /// This eliminates nested loops by pre-processing offers once.
        /// </summary>
        /// <param name="productIds">Products in the cart</param>
        /// <returns>Dictionary mapping ProductId to List of applicable Offers</returns>
        private async Task<Dictionary<Guid, List<Offer>>> BuildProductOffersLookup(List<Guid> productIds)
        {
            var offerRepo = _unitOfWork.Repository<Offer>();
            var now = DateTime.UtcNow;

            var activeOffers = await offerRepo.GetAllAsync(
                o => o.IsActive && o.StartDate <= now && o.EndDate >= now,
                o => o.OfferProducts);

            var lookup = new Dictionary<Guid, List<Offer>>();

            foreach (var offer in activeOffers)
            {
                foreach (var op in offer.OfferProducts)
                {
                    if (!productIds.Contains(op.ProductId))
                        continue;

                    if (!lookup.TryGetValue(op.ProductId, out var list))
                    {
                        list = new List<Offer>();
                        lookup[op.ProductId] = list;
                    }

                    list.Add(offer);
                }
            }

            return lookup;
        }
    }
}
