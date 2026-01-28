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
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IWebsiteProductRepository productRepository,
            ITenantProvider tenantProvider,
            IPricingService pricingService,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
            _pricingService = pricingService;
            _mediator = mediator;
        }

        public async Task<CreateOrderCommandResponse> Handle(
            CreateOrderCommandRequest request,
            CancellationToken cancellationToken)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderItemRepo = _unitOfWork.Repository<OrderItem>();
            var offerRepo = _unitOfWork.Repository<Offer>();

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

            // 3?? Load active offers
            var now = DateTime.UtcNow;
            var activeOffers = await offerRepo.GetAllAsync(
                o => o.IsActive && o.StartDate <= now && o.EndDate >= now,
                o => o.OfferProducts);

            // 4?? Build offers lookup
            var offersLookup = new Dictionary<Guid, List<Offer>>();

            foreach (var offer in activeOffers)
            {
                foreach (var op in offer.OfferProducts)
                {
                    if (!productIds.Contains(op.ProductId))
                        continue;

                    if (!offersLookup.TryGetValue(op.ProductId, out var list))
                    {
                        list = new List<Offer>();
                        offersLookup[op.ProductId] = list;
                    }

                    list.Add(offer);
                }
            }

            var tenantId = _tenantProvider.GetTenantId()!;
            decimal subTotal = 0;
            decimal discountTotal = 0;

            // 5?? Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = request.UserId,
                Status = OrderStatus.Pending,
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

            // 6?? Create order items with FINAL pricing
            foreach (var cartItem in cart.Items)
            {
                var product = products.First(p => p.Id == cartItem.ProductId);

                var applicableOffers = offersLookup.TryGetValue(
                    cartItem.ProductId, out var list)
                    ? list
                    : new List<Offer>();

                var pricing = _pricingService.CalculateBestOffer(
                    cartItem.UnitPrice,
                    cartItem.Quantity,
                    applicableOffers);

                subTotal += pricing.OriginalPrice;
                discountTotal += pricing.DiscountAmount;

                await orderItemRepo.AddAsync(new OrderItem
                {
                    OrderId = order.Id,
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

            // 7?? Final totals
            order.SubTotal = subTotal;
            order.DiscountTotal = discountTotal;
            order.TotalAmount = subTotal - discountTotal;

            // 8?? Checkout cart
            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;
            cartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 9?? Publish inventory event
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
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..8].ToUpper();
        }
    }
}
