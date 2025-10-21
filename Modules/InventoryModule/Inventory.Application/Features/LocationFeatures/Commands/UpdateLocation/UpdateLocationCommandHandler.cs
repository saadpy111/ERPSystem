using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.UpdateLocation
{
    public class UpdateLocationCommandHandler 
        : IRequestHandler<UpdateLocationCommandRequest, UpdateLocationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateLocationCommandResponse> Handle(UpdateLocationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Location>();
                var entity = await repo.GetById(request.Location.Id);
                
                if (entity == null)
                    return new UpdateLocationCommandResponse { Success = false };

                entity.Name = request.Location.Name;
                entity.Type = request.Location.Type;
                entity.WarehouseId = request.Location.WarehouseId;
                entity.ParentId = request.Location.ParentId;
                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
                await _unitOfWork.CompleteAsync();

                return new UpdateLocationCommandResponse
                {
                    Success = true,
                    Location = new GetLocationDto
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Type = entity.Type,
                        WarehouseId = entity.WarehouseId,
                        ParentId = entity.ParentId,
                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt
                    }
                };
            }
            catch
            {
                return new UpdateLocationCommandResponse { Success = false };
            }
        }
    }
}