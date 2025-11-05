using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;
using Inventory.Application.Contracts.Infrastruture.FileService;
using System.Linq;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommandRequest, CreateWarehouseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<CreateWarehouseCommandResponse> Handle(CreateWarehouseCommandRequest request, CancellationToken cancellationToken)
        {

            try
            {
                var entity = new Warehouse
                {
                    Name = request.Warehouse.Name,
                    LocationDetails = request.Warehouse.LocationDetails,
                    WarehouseCode = request.Warehouse.WarehouseCode,
                    ResponsibleEmployee = request.Warehouse.ResponsibleEmployee,
                    ContactNumber = request.Warehouse.ContactNumber,
                    IsActive = request.Warehouse.IsActive,
                    WarehouseType = request.Warehouse.WarehouseType,
                    FinancialAccountCode = request.Warehouse.FinancialAccountCode,
                    PercentageUtilized = request.Warehouse.PercentageUtilized,
                    TotalStorageCapacity = request.Warehouse.TotalStorageCapacity,
                    InventoryPolicy = request.Warehouse.InventoryPolicy,
                    Government = request.Warehouse.Government
                };
                await _unitOfWork.Repositories<Warehouse>().Add(entity);
                await _unitOfWork.CompleteAsync();

                // attachments: save files and create attachment records
                if (request.Warehouse.Attachments != null && request.Warehouse.Attachments.Any())
                {
                    foreach (var attachmentDto in request.Warehouse.Attachments)
                    {
                        var filePath = await _fileService.SaveFileAsync(attachmentDto.File, "warehouseAttachments");
                        var attachment = new Attachment
                        {
                            FileName = attachmentDto.File.FileName,
                            FileUrl = filePath,
                            ContentType = attachmentDto.File.ContentType,
                            FileSize = attachmentDto.File.Length,
                            EntityType = nameof(Warehouse),
                            EntityId = entity.Id,
                            Description = attachmentDto.Description,
                            UploadedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.Repositories<Attachment>().Add(attachment);
                    }
                }
                await _unitOfWork.CompleteAsync();

                return new CreateWarehouseCommandResponse
                {
                    Success = true,
                    Message = "Created"
                };
            }
            catch
            {
                return new CreateWarehouseCommandResponse
                {
                    Success = false,
                    Message = "Failed"

                };


            }
        }
    }
}