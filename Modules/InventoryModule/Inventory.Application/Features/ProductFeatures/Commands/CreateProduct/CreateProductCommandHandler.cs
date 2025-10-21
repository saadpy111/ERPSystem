using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using MediatR;

namespace Inventory.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Product
                {
                    Sku = request.Product.Sku,
                    Name = request.Product.Name,
                    Description = request.Product.Description ?? string.Empty,
                    UnitOfMeasure = request.Product.UnitOfMeasure,
                    SalePrice = request.Product.SalePrice,
                    CostPrice = request.Product.CostPrice,
                    IsActive = true,
                    CategoryId = request.Product.CategoryId,
                    CreatedAt = DateTime.UtcNow
                };

                // attribute values
                if (request.Product.AttributeValues != null && request.Product.AttributeValues.Any())
                {
                    entity.AttributeValues = request.Product.AttributeValues.Select(av => new ProductAttributeValue
                    {
                        ProductId = entity.Id,
                        AttributeId = av.AttributeId,
                        Value = av.Value,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }

                // images: save files and attach
                if (request.Product.Images != null && request.Product.Images.Any())
                {
                    var images = new List<ProductImage>();
                    foreach (var imageDto in request.Product.Images)
                    {
                        var imagePath = await _fileService.SaveFileAsync(imageDto.Image, "productImages");
                        var img = new ProductImage
                        {
                            ProductId = entity.Id,
                            ImageUrl = imagePath,
                            Description = imageDto.Description,
                            IsPrimary = imageDto.IsPrimary,
                            ImageType = imageDto.ImageType,
                            DisplayOrder = imageDto.DisplayOrder,
                            CreatedAt = DateTime.UtcNow
                        };
                        images.Add(img);
                    }
                    entity.Images = images;
                }
                await _unitOfWork.Repositories<Product>().Add(entity);

                await _unitOfWork.CompleteAsync();

                var dto = entity.ToDto();

                return new CreateProductCommandResponse
                {
                    Success = true,
                    Product = dto
                };
            }
            catch
            {
                return new CreateProductCommandResponse { Success = false };
            }
        }
    }
}