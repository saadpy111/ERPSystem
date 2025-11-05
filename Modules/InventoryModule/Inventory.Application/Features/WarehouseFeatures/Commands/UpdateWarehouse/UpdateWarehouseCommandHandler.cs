using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;
using Inventory.Application.Contracts.Infrastruture.FileService;
using System.Linq;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommandRequest, UpdateWarehouseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<UpdateWarehouseCommandResponse> Handle(UpdateWarehouseCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Warehouse>();
                var entity = await repo.GetById(request.Warehouse.Id);
                if (entity == null) return null;

                entity.Name = request.Warehouse.Name;
                entity.LocationDetails = request.Warehouse.LocationDetails;
                entity.WarehouseCode = request.Warehouse.WarehouseCode;
                entity.ResponsibleEmployee = request.Warehouse.ResponsibleEmployee;
                entity.ContactNumber = request.Warehouse.ContactNumber;
                entity.IsActive = request.Warehouse.IsActive;
                entity.WarehouseType = request.Warehouse.WarehouseType;
                entity.FinancialAccountCode = request.Warehouse.FinancialAccountCode;
                entity.PercentageUtilized = request.Warehouse.PercentageUtilized;
                entity.TotalStorageCapacity = request.Warehouse.TotalStorageCapacity;
                entity.InventoryPolicy = request.Warehouse.InventoryPolicy;
                entity.Government = request.Warehouse.Government;

                // update attachments
                if (request.Warehouse.EditAttachments)
                {
                    var oldAttachments = await _unitOfWork.Repositories<Attachment>()
                    .GetAll(a => a.EntityType == nameof(Warehouse) && a.EntityId == entity.Id);

                    foreach (var att in oldAttachments)
                    {
                        await _fileService.DeleteFileAsync(att.FileUrl);
                        _unitOfWork.Repositories<Attachment>().Remove(att);
                    }

                    if (request.Warehouse.Attachments?.Any() == true)
                    {
                        foreach (var attdto in request.Warehouse.Attachments)
                        {
                            var path = await _fileService.SaveFileAsync(attdto.File, "warehouseAttachments");

                            var newAttachment = new Attachment
                            {
                                FileName = attdto.File.FileName,
                                FileUrl = path,
                                ContentType = attdto.File.ContentType,
                                FileSize = attdto.File.Length,
                                EntityType = nameof(Warehouse),
                                EntityId = entity.Id,
                                Description = attdto.Description,
                                UploadedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow
                            };

                            await _unitOfWork.Repositories<Attachment>().Add(newAttachment);
                        }
                    }
                }

                repo.Update(entity);
                await _unitOfWork.CompleteAsync();

                return new UpdateWarehouseCommandResponse()
                {
                    Success = true,
                    Message = "Updated"
                };
            }
            catch
            {
                return new UpdateWarehouseCommandResponse()
                {
                    Success = false,
                    Message = "Failed"
                };
            }

        }
    }
}