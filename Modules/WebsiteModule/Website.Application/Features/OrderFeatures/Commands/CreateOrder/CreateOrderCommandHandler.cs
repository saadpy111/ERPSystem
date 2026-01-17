using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using Events.WebsiteEvents;

namespace Website.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IWebsiteProductRepository productRepository,
            ITenantProvider tenantProvider,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
            _mediator = mediator;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderItemRepo = _unitOfWork.Repository<OrderItem>();

            // Get cart
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                asNoTracking: false,
                c => c.Items);

            if (cart == null || !cart.Items.Any())
            {
                return new CreateOrderCommandResponse
                {
                    Success = false,
                    Message = "Cart is empty."
                };
            }

            // Load products to validate availability
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = await _productRepository.GetAllAsync(p => productIds.Contains(p.Id));

            var unavailableProducts = products.Where(p => !p.IsAvailable || !p.IsPublished).ToList();
            if (unavailableProducts.Any())
            {
                return new CreateOrderCommandResponse
                {
                    Success = false,
                    Message = $"Products unavailable: {string.Join(", ", unavailableProducts.Select(p => p.NameSnapshot))}"
                };
            }

            var tenantId = _tenantProvider.GetTenantId() ?? string.Empty;

            // Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = request.UserId,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.UnitPrice),
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

            // Create order items
            foreach (var cartItem in cart.Items)
            {
                var product = products.First(p => p.Id == cartItem.ProductId);
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductNameSnapshot = product.NameSnapshot,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TenantId = tenantId
                };
                await orderItemRepo.AddAsync(orderItem);
            }

            // Mark cart as checked out
            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;
            cartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish event for inventory
            var orderEvent = new OrderCreatedEvent
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
            };

            await _mediator.Publish(orderEvent, cancellationToken);

            return new CreateOrderCommandResponse
            {
                Success = true,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber
            };
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
