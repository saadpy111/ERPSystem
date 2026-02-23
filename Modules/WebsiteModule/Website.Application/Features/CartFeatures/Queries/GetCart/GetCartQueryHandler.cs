using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Services;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.CartFeatures.Queries.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQueryRequest, GetCartQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IPricingService _pricingService;
        private readonly IOfferEligibilityService _offerEligibilityService;
        private readonly ICouponService _couponService;

        public GetCartQueryHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider,
            IPricingService pricingService,
            IOfferEligibilityService offerEligibilityService,
            ICouponService couponService)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _pricingService = pricingService;
            _offerEligibilityService = offerEligibilityService;
            _couponService = couponService;
        }

        public async Task<GetCartQueryResponse> Handle(
            GetCartQueryRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId() ?? string.Empty;

            var cartRepo = _unitOfWork.Repository<Cart>();
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                asNoTracking: true,
                c => c.Items);

            if (cart == null)
            {
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

            // Load products
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var productIds = cart.Items.Select(i => i.ProductId).ToList();

            var products = await productRepo.GetAllAsync(
                p => productIds.Contains(p.Id),
                p => p.Images);

            var productsDict = products.ToDictionary(p => p.Id);

            // 1. Handle Coupon Preview
            Coupon? couponToPreview = null;
            if (!string.IsNullOrWhiteSpace(request.CouponCode))
            {
                var cartSubtotal = cart.Items.Sum(i => i.Quantity * i.UnitPrice);
                var validationResult = await _couponService.ValidateCouponAsync(
                    request.CouponCode,
                    request.UserId,
                    cartSubtotal,
                    cancellationToken);

                if (validationResult.IsValid)
                {
                    couponToPreview = validationResult.Coupon;
                }
            }

            // 2. Build Offers Lookup (Skip if exclusive coupon)
            var skipOffers = couponToPreview != null && !couponToPreview.CanBeCombinedWithOffers;
            var offersLookup = skipOffers
                ? new Dictionary<Guid, List<Offer>>()
                : await _offerEligibilityService.BuildProductOffersLookup(products, tenantId, cancellationToken);

            var cartItems = new List<CartItemDto>();
            decimal subtotal = 0;
            decimal totalOfferDiscount = 0;

            foreach (var item in cart.Items)
            {
                var product = productsDict.ContainsKey(item.ProductId) ? productsDict[item.ProductId] : null;

                var lineSubtotal = item.Quantity * item.UnitPrice;
                subtotal += lineSubtotal;

                var applicableOffers = offersLookup.TryGetValue(item.ProductId, out var list) ? list : new List<Offer>();

                var pricing = _pricingService.CalculateBestOffer(
                    item.UnitPrice,
                    item.Quantity,
                    applicableOffers);

                totalOfferDiscount += pricing.DiscountAmount;

                cartItems.Add(new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product?.NameSnapshot ?? "Unknown",
                    ProductImageUrl = product?.Images?.FirstOrDefault()?.ImagePath,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = lineSubtotal,
                    EstimatedDiscount = pricing.DiscountAmount,
                    EstimatedFinalPrice = pricing.FinalPrice,
                    AppliedOfferName = pricing.AppliedOfferName,
                    IsAvailable = product?.IsAvailable ?? false
                });
            }

            // 3. Calculate Estimated Coupon Discount
            decimal couponDiscountAmount = 0;
            if (couponToPreview != null)
            {
                var amountAfterOffers = subtotal - totalOfferDiscount;
                couponDiscountAmount = _couponService.CalculateCouponDiscount(couponToPreview, amountAfterOffers);
            }

            return new GetCartQueryResponse
            {
                Cart = new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    Items = cartItems,
                    Subtotal = subtotal,
                    EstimatedDiscountTotal = totalOfferDiscount,
                    EstimatedCouponDiscount = couponDiscountAmount,
                    AppliedCouponCode = couponToPreview?.Code,
                    EstimatedTotal = subtotal - (totalOfferDiscount + couponDiscountAmount)
                }
            };
        }
    }
}