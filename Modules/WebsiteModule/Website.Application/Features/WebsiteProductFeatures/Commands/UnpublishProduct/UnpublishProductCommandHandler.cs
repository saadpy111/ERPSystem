using MediatR;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UnpublishProduct
{
    public class UnpublishProductCommandHandler : IRequestHandler<UnpublishProductCommandRequest, UnpublishProductCommandResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishProductCommandHandler(
            IWebsiteProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnpublishProductCommandResponse> Handle(UnpublishProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.WebsiteProductId);
            if (product == null)
            {
                return new UnpublishProductCommandResponse
                {
                    Success = false,
                    Message = "Website product not found."
                };
            }

            // Idempotent: if already unpublished, just return success
            if (!product.IsPublished)
            {
                return new UnpublishProductCommandResponse
                {
                    Success = true,
                    Message = "Product is already unpublished."
                };
            }

            // Set IsPublished = false, preserve snapshot
            product.IsPublished = false;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UnpublishProductCommandResponse { Success = true };
        }
    }
}
