using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Domain.Entities;

namespace Website.Application.Features.FavoriteFeatures.Commands.AddFavorite
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, AddFavoriteResponse>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly ITenantProvider _tenantProvider;

        public AddFavoriteCommandHandler(
            IFavoriteRepository favoriteRepository,
            IWebsiteProductRepository productRepository,
            ITenantProvider tenantProvider)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
        }

        public async Task<AddFavoriteResponse> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new AddFavoriteResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            if (!product.IsPublished)
            {
                return new AddFavoriteResponse
                {
                    Success = false,
                    Message = "Product is not published and cannot be added to favorites."
                };
            }

            var exists = await _favoriteRepository.ExistsAsync(request.UserId, request.ProductId, cancellationToken);
            if (exists)
            {
                return new AddFavoriteResponse
                {
                    Success = true,
                    Message = "Product added to favorites."
                };
            }

            var favorite = new FavoriteProduct
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                TenantId = _tenantProvider.GetTenantId()!
            };

            await _favoriteRepository.AddAsync(favorite);
            await _favoriteRepository.SaveChangesAsync(cancellationToken);

            return new AddFavoriteResponse
            {
                Success = true,
                Message = "Product added to favorites."
            };
        }
    }
}
