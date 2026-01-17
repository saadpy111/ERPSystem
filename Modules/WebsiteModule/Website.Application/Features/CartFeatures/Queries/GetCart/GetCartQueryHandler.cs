using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Queries.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQueryRequest, GetCartQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetCartQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
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
                        Total = 0
                    }
                };
            }

            // Load products for items
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = await productRepo.GetAllAsync(p => productIds.Contains(p.Id));

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i =>
                {
                    var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                    return new CartItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = product?.NameSnapshot ?? "Unknown",
                        ProductImageUrl = product?.ImageUrlSnapshot,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.Quantity * i.UnitPrice,
                        IsAvailable = product?.IsAvailable ?? false
                    };
                }).ToList(),
                Total = cart.Items.Sum(i => i.Quantity * i.UnitPrice)
            };

            return new GetCartQueryResponse { Cart = cartDto };
        }
    }
}
