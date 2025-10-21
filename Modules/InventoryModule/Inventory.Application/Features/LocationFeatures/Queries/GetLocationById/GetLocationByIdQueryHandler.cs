using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationById
{
    public class GetLocationByIdQueryHandler 
        : IRequestHandler<GetLocationByIdQueryRequest, GetLocationByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetLocationByIdQueryResponse> Handle(GetLocationByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Repositories<Location>()
                .GetById(request.Id, l => l.ChildLocations);

            if (location == null)
                return new GetLocationByIdQueryResponse();

            var dto = new GetLocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Type = location.Type,
                WarehouseId = location.WarehouseId,
                ParentId = location.ParentId,
                CreatedAt = location.CreatedAt,
                UpdatedAt = location.UpdatedAt,
                ChildLocations = location.ChildLocations?.Select(c => new GetLocationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    WarehouseId = c.WarehouseId,
                    ParentId = c.ParentId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList()
            };

            return new GetLocationByIdQueryResponse { Location = dto };
        }
    }
}