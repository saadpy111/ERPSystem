using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommandRequest, AddToCartCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly ITenantProvider _tenantProvider;

        public AddToCartCommandHandler(
            IUnitOfWork unitOfWork,
            IWebsiteProductRepository productRepository,
            ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
        }

        public async Task<AddToCartCommandResponse> Handle(AddToCartCommandRequest request, CancellationToken cancellationToken)
        {
            // Validate product
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null || !product.IsPublished)
            {
                return new AddToCartCommandResponse
                {
                    Success = false,
                    Message = "Product not found or not available."
                };
            }

            var cartRepo = _unitOfWork.Repository<Cart>();
            var cartItemRepo = _unitOfWork.Repository<CartItem>();

            // Get or create cart
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                asNoTracking: false,
                c => c.Items);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = request.UserId,
                    TenantId = _tenantProvider.GetTenantId() ?? string.Empty
                };
                await cartRepo.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // Check if product already in cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                cartItemRepo.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price,
                    TenantId = _tenantProvider.GetTenantId() ?? string.Empty
                };
                await cartItemRepo.AddAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddToCartCommandResponse
            {
                Success = true,
                CartId = cart.Id
            };
        }
    }
}
