using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;
using Inventory.Application.Dtos.AttachmentDtos;
using System.Linq;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetWarehouseById
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQueryRequest, GetWarehouseByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetWarehouseByIdQueryResponse> Handle(GetWarehouseByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<Warehouse>().GetById(request.Id);
            if (entity == null) return null;

            // Get Attachments related to this Warehouse (Polymorphic)
            var attachments = await _unitOfWork.Repositories<Attachment>()
                .GetAll(a => a.EntityType == nameof(Warehouse) && a.EntityId == entity.Id);

            var dto =  new GetWarehouseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.LocationDetails,
                WarehouseCode = entity.WarehouseCode,
                ResponsibleEmployee = entity.ResponsibleEmployee,
                ContactNumber = entity.ContactNumber,
                IsActive = entity.IsActive,
                WarehouseType = entity.WarehouseType,
                FinancialAccountCode = entity.FinancialAccountCode,
                PercentageUtilized = entity.PercentageUtilized,
                TotalStorageCapacity = entity.TotalStorageCapacity,
                InventoryPolicy = entity.InventoryPolicy,
                Government = entity.Government
            };

            if (attachments != null && attachments.Any())
            {
                dto.Attachments = attachments.Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType,
                    Description = a.Description,
                    UploadedAt = a.UploadedAt
                }).ToList();
            }
            else
            {
                dto.Attachments = new List<AttachmentDto>();
            }

            return new GetWarehouseByIdQueryResponse()
            {
                WarehouseDto = dto
            };
        }
    }
}