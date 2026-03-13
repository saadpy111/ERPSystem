using MediatR;
using SharedKernel.Contracts;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.RepublishProduct
{
    public class RepublishProductCommandHandler : IRequestHandler<RepublishProductCommandRequest, RepublishProductCommandResponse>
    {
        private readonly IInventoryReadService _inventoryReadService;
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RepublishProductCommandHandler(
            IInventoryReadService inventoryReadService,
            IWebsiteProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _inventoryReadService = inventoryReadService;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RepublishProductCommandResponse> Handle(RepublishProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithImagesAsync(request.WebsiteProductId);
            if (product == null)
            {
                return new RepublishProductCommandResponse
                {
                    Success = false,
                    Message = "Website product not found."
                };
            }

            product.IsPublished = true;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RepublishProductCommandResponse
            {
                Success = true,
                Message = "Product republished successfully."
            };
        }
    }
}
