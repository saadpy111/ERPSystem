using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq;

namespace Inventory.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Product>();
                var product = await repo.GetById(request.Product.Id, p => p.AttributeValues, p => p.Images);
                if (product == null)
                    return new UpdateProductCommandResponse { Success = false };

                #region update core fields
                product.Sku = request.Product.Sku;
                product.Name = request.Product.Name;
                product.Description = request.Product.Description ?? string.Empty;
                product.UnitOfMeasure = request.Product.UnitOfMeasure;
                product.UpdatedAt = DateTime.UtcNow;


                #region track cost change
                if (product.CostPrice != request.Product.CostPrice)
                {
                    await _unitOfWork.Repositories<ProductCostHistory>().Add(new ProductCostHistory
                    {
                        OldCost = product.CostPrice,
                        NewCost = request.Product.CostPrice,
                        ProductId = product.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                #endregion

                product.SalePrice = request.Product.SalePrice;
                product.CostPrice = request.Product.CostPrice;
                product.IsActive = request.Product.IsActive;
                product.CategoryId = request.Product.CategoryId;
                product.ProductBarcode = request.Product.ProductBarcode;
                product.MainSupplierName = request.Product.MainSupplierName;
                product.Tax = request.Product.Tax;
                product.OrderLimit = request.Product.OrderLimit;
                #endregion


                #region update attribute values (replace)
                var updatedAttrValues = request.Product.AttributeValues?.Select(av => new ProductAttributeValue
                {
                    Id = av.Id,
                    ProductId = product.Id,
                    AttributeId = av.AttributeId,
                    Value = av.Value,
                    CreatedAt = av.Id == Guid.Empty ? DateTime.UtcNow : (DateTime?)null
                }).ToList() ?? new List<ProductAttributeValue>();

                product.AttributeValues = updatedAttrValues;
                #endregion

                #region updateimages
                if (request.Product.Images != null)
                {
                    var oldImageIds = product.Images.Select(i => i.Id).ToList();

                    var incomingIds = request.Product.Images
                        .Where(i => i.Id.HasValue)
                        .Select(i => i.Id.Value)
                        .ToList();

                    var deletedImages = product.Images
                        .Where(i => !incomingIds.Contains(i.Id))
                        .ToList();

                    foreach (var img in deletedImages)
                    {
                        await _fileService.DeleteFileAsync(img.ImageUrl);
                        product.Images.Remove(img);
                    }

                    if (request.Product.Images.Any())
                    {
                        product.Images ??= new List<ProductImage>();

                        foreach (var imageDto in request.Product.Images)
                        {
                            if (imageDto.Id.HasValue)
                            {
                                var existingImage = product.Images.FirstOrDefault(i => i.Id == imageDto.Id.Value);
                                if (existingImage != null)
                                {
                                    if (imageDto.Image != null)
                                    {
                                        if (!string.IsNullOrEmpty(existingImage.ImageUrl))
                                            await _fileService.DeleteFileAsync(existingImage.ImageUrl);

                                        var newPath = await _fileService.SaveFileAsync(imageDto.Image, "productImages");
                                        existingImage.ImageUrl = newPath;
                                    }

                                    existingImage.Description = imageDto.Description;
                                    existingImage.DisplayOrder = imageDto.DisplayOrder;
                                    existingImage.IsPrimary = imageDto.IsPrimary;
                                    existingImage.UpdatedAt = DateTime.UtcNow;
                                }
                            }
                            else
                            {
                                if (imageDto.Image != null)
                                {
                                    var path = await _fileService.SaveFileAsync(imageDto.Image, "productImages");
                                    await _unitOfWork.Repositories<ProductImage>().Add(new ProductImage
                                    {
                                        ProductId = product.Id,
                                        ImageUrl = path,
                                        Description = imageDto.Description,
                                        IsPrimary = imageDto.IsPrimary,
                                        DisplayOrder = imageDto.DisplayOrder,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                }
                            }
                        }
                    }
                }
                #endregion


                #region update attachments
                if (request.Product.EditAttachments)
                {
                    var oldAttachments = await _unitOfWork.Repositories<Attachment>()
                    .GetAll(a => a.EntityType == nameof(Product) && a.EntityId == product.Id);

                    foreach (var att in oldAttachments)
                    {
                        await _fileService.DeleteFileAsync(att.FileUrl);
                        _unitOfWork.Repositories<Attachment>().Remove(att);
                    }

                    if (request.Product.Attachments?.Any() == true)
                    {
                        foreach (var attdto in request.Product.Attachments)
                        {
                            var path = await _fileService.SaveFileAsync(attdto.File, "productAttachments");

                            var newAttachment = new Attachment
                            {
                                FileName = attdto.File.FileName,
                                FileUrl = path,
                                ContentType = attdto.File.ContentType,
                                FileSize = attdto.File.Length,
                                EntityType = nameof(Product),
                                EntityId = product.Id,
                                Description = attdto.Description,
                                UploadedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow
                            };

                            await _unitOfWork.Repositories<Attachment>().Add(newAttachment);
                        }
                    }
                }
                #endregion


                
                repo.Update(product);
                await _unitOfWork.CompleteAsync();

                var dto = product.ToDto();

                return new UpdateProductCommandResponse { Success = true, Product = dto };
            }
            catch(Exception)
            {
               
                return new UpdateProductCommandResponse { Success = false };
            }
        }
    }
}