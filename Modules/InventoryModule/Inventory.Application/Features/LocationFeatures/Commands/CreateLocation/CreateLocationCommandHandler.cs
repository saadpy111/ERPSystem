using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.CreateLocation
{
    public class CreateLocationCommandHandler 
        : IRequestHandler<CreateLocationCommandRequest, CreateLocationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateLocationCommandResponse> Handle(CreateLocationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Location
                {
                    Name = request.Location.Name,
                    Type = request.Location.Type,
                    WarehouseId = request.Location.WarehouseId,
                    ParentId = request.Location.ParentId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repositories<Location>().Add(entity);
                await _unitOfWork.CompleteAsync();

                return new CreateLocationCommandResponse
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
                return new CreateLocationCommandResponse { Success = false };
            }
        }
    }
}