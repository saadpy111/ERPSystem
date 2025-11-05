using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using MediatR;
using System.Linq;

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
                    ProductBarcode = request.Product.ProductBarcode,
                    MainSupplierName = request.Product.MainSupplierName,
                    Tax = request.Product.Tax,
                    OrderLimit = request.Product.OrderLimit,
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
                            DisplayOrder = imageDto.DisplayOrder,
                            CreatedAt = DateTime.UtcNow
                        };
                        images.Add(img);
                    }
                    entity.Images = images;
                }
                
                // attachments: save files and store attachment IDs
                if (request.Product.Attachments != null && request.Product.Attachments.Any())
                {
                    foreach (var attachmentDto in request.Product.Attachments)
                    {
                        var filePath = await _fileService.SaveFileAsync(attachmentDto.File, "productAttachments");
                        var attachment = new Attachment
                        {
                            FileName = attachmentDto.File.FileName,
                            FileUrl = filePath,
                            ContentType = attachmentDto.File.ContentType,
                            FileSize = attachmentDto.File.Length,
                            EntityType = nameof(Product),
                            EntityId = entity.Id,
                            Description = attachmentDto.Description,
                            UploadedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.Repositories<Attachment>().Add(attachment);
                        //attachmentIds.Add(attachment.Id);
                    }
                    //entity.AttachmentIds = string.Join(",", attachmentIds);
                }
                
                // Create initial stock quant if provided
                if (request.Product.InitialStockQuant != null)
                {
                    var stockQuant = new StockQuant
                    {
                        ProductId = entity.Id,
                        LocationId = request.Product.InitialStockQuant.LocationId,
                        Quantity = request.Product.InitialStockQuant.Quantity,
                        ReservedQuantity = 0,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.Repositories<StockQuant>().Add(stockQuant);
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