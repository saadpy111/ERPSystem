using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using Events.WebsiteEvents;
using Website.Application.Services;

namespace Website.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderCommandHandler
        : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IPricingService _pricingService;
        private readonly IOfferEligibilityService _offerEligibilityService;
        private readonly ICouponService _couponService;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IWebsiteProductRepository productRepository,
            ITenantProvider tenantProvider,
            IPricingService pricingService,
            IOfferEligibilityService offerEligibilityService,
            ICouponService couponService,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
            _pricingService = pricingService;
            _offerEligibilityService = offerEligibilityService;
            _couponService = couponService;
            _mediator = mediator;
        }

        public async Task<CreateOrderCommandResponse> Handle(
            CreateOrderCommandRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId()!;
            var cartRepo = _unitOfWork.Repository<Cart>();
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderItemRepo = _unitOfWork.Repository<OrderItem>();

            // 1?? Load cart
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                false,
                c => c.Items);

            if (cart == null || !cart.Items.Any())
            {
                return new CreateOrderCommandResponse
                {
                    Success = false,
                    Message = "Cart is empty."
                };
            }

            // 2?? Load products
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = await _productRepository.GetAllAsync(
                p => productIds.Contains(p.Id));

            if (products.Any(p => !p.IsAvailable || !p.IsPublished))
            {
                return new CreateOrderCommandResponse
                {
                    Success = false,
                    Message = "One or more products are unavailable."
                };
            }

            // 3?? Handle Coupon Validation
            Coupon? appliedCoupon = null;
            if (!string.IsNullOrWhiteSpace(request.CouponCode))
            {
                var cartSubtotal = cart.Items.Sum(i => i.Quantity * i.UnitPrice);
                var validationResult = await _couponService.ValidateCouponAsync(
                    request.CouponCode,
                    request.UserId,
                    cartSubtotal,
                    cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new CreateOrderCommandResponse
                    {
                        Success = false,
                        Message = validationResult.Message
                    };
                }

                appliedCoupon = validationResult.Coupon;
            }

            // 4?? Build offers lookup (Skip if coupon is exclusive)
            var skipOffers = appliedCoupon != null && !appliedCoupon.CanBeCombinedWithOffers;
            var offersLookup = skipOffers 
                ? new Dictionary<Guid, List<Offer>>()
                : await _offerEligibilityService.BuildProductOffersLookup(products, tenantId, cancellationToken);

            decimal totalOriginalPrice = 0;
            decimal totalOfferDiscount = 0;
            var orderItemsToCreate = new List<OrderItem>();

            // 5?? Calculate line-level pricing
            foreach (var cartItem in cart.Items)
            {
                var product = products.First(p => p.Id == cartItem.ProductId);
                var applicableOffers = offersLookup.TryGetValue(cartItem.ProductId, out var list) ? list : new List<Offer>();

                var pricing = _pricingService.CalculateBestOffer(
                    cartItem.UnitPrice,
                    cartItem.Quantity,
                    applicableOffers);

                totalOriginalPrice += pricing.OriginalPrice;
                totalOfferDiscount += pricing.DiscountAmount;

                orderItemsToCreate.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductNameSnapshot = product.NameSnapshot,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    DiscountAmount = pricing.DiscountAmount,
                    FinalPrice = pricing.FinalPrice,
                    AppliedOfferName = pricing.AppliedOfferName,
                    TenantId = tenantId
                });
            }

            // 6?? Calculate Order-level Coupon Discount
            decimal couponDiscount = 0;
            if (appliedCoupon != null)
            {
                var amountToDiscount = totalOriginalPrice - totalOfferDiscount;
                couponDiscount = _couponService.CalculateCouponDiscount(appliedCoupon, amountToDiscount);
            }

            // 7?? Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = request.UserId,
                Status = OrderStatus.Pending,
                SubTotal = totalOriginalPrice,
                DiscountTotal = totalOfferDiscount + couponDiscount,
                AppliedCouponCode = appliedCoupon?.Code,
                CouponDiscountAmount = couponDiscount,
                TotalAmount = totalOriginalPrice - (totalOfferDiscount + couponDiscount),
                PaymentMethod = request.PaymentMethod,
                ShippingAddress = new ShippingAddress(
                    request.Street,
                    request.City,
                    request.State,
                    request.Country,
                    request.ZipCode),
                Notes = request.Notes,
                TenantId = tenantId
            };

            await orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 8?? Save items and usage
            foreach (var item in orderItemsToCreate)
            {
                item.OrderId = order.Id;
                await orderItemRepo.AddAsync(item);
            }

            if (appliedCoupon != null)
            {
                await _unitOfWork.Repository<CouponUsage>().AddAsync(new CouponUsage
                {
                    CouponId = appliedCoupon.Id,
                    UserId = request.UserId,
                    OrderId = order.Id,
                    UsedAt = DateTime.UtcNow,
                    TenantId = tenantId
                });
            }

            // 9?? Checkout cart
            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;
            cartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 10?? Publish event
            await _mediator.Publish(new OrderCreatedEvent
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                TenantId = tenantId,
                Items = cart.Items.Select(i =>
                {
                    var product = products.First(p => p.Id == i.ProductId);
                    return new OrderItemInfo
                    {
                        InventoryProductId = product.InventoryProductId,
                        Quantity = i.Quantity
                    };
                }).ToList()
            }, cancellationToken);

            return new CreateOrderCommandResponse
            {
                Success = true,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber
            };
        }

        private static string GenerateOrderNumber()
        {
            var time = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var unique = Guid.NewGuid().ToString("N")[..6].ToUpper();

            return $"ORD-{time}-{unique}";
        }
    }
}
