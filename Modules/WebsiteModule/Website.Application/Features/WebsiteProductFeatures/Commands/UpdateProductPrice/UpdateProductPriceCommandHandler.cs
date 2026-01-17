using MediatR;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandHandler : IRequestHandler<UpdateProductPriceCommandRequest, UpdateProductPriceCommandResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductPriceCommandHandler(
            IWebsiteProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductPriceCommandResponse> Handle(UpdateProductPriceCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new UpdateProductPriceCommandResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            product.Price = request.NewPrice;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateProductPriceCommandResponse { Success = true };
        }
    }
}
