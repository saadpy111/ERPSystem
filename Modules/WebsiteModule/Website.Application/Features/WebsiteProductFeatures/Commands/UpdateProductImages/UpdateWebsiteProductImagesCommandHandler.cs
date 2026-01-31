using MediatR;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductImages
{
    public class UpdateWebsiteProductImagesCommandHandler : IRequestHandler<UpdateWebsiteProductImagesCommandRequest, UpdateWebsiteProductImagesCommandResponse>
    {
        private readonly IWebsiteProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateWebsiteProductImagesCommandHandler(
            IWebsiteProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<UpdateWebsiteProductImagesCommandResponse> Handle(UpdateWebsiteProductImagesCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithImagesAsync(request.ProductId);
            if (product == null)
            {
                return new UpdateWebsiteProductImagesCommandResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            foreach (var imageDto in request.Images)
            {
                if (imageDto.IsDeleted && imageDto.Id.HasValue)
                {
                    var existingImage = product.Images.FirstOrDefault(i => i.Id == imageDto.Id.Value);
                    if (existingImage != null)
                    {
                        await _fileService.DeleteFileAsync(existingImage.ImagePath);
                        product.Images.Remove(existingImage);
                    }
                    continue;
                }

                if (imageDto.Id.HasValue)
                {
                    var existingImage = product.Images.FirstOrDefault(i => i.Id == imageDto.Id.Value);
                    if (existingImage != null)
                    {
                        if (imageDto.Image != null)
                        {
                            await _fileService.DeleteFileAsync(existingImage.ImagePath);
                            var newPath = await _fileService.SaveFileAsync(imageDto.Image, "websiteproducts");
                            existingImage.ImagePath = newPath;
                        }

                        existingImage.AltText = imageDto.AltText;
                        existingImage.IsPrimary = imageDto.IsPrimary;
                        existingImage.DisplayOrder = imageDto.DisplayOrder;
                        existingImage.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else if (imageDto.Image != null)
                {
                    var path = await _fileService.SaveFileAsync(imageDto.Image, "websiteproducts");
                    product.Images.Add(new WebsiteProductImage
                    {
                        ImagePath = path,
                        AltText = imageDto.AltText,
                        IsPrimary = imageDto.IsPrimary,
                        DisplayOrder = imageDto.DisplayOrder,
                        TenantId = product.TenantId
                    });
                }
            }

            product.UpdatedAt = DateTime.UtcNow;
            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateWebsiteProductImagesCommandResponse
            {
                Success = true,
                Message = "Product images updated successfully."
            };
        }
    }
}
