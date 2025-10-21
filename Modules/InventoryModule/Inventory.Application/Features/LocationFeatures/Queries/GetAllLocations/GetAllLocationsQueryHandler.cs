using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetAllLocations
{
    public class GetAllLocationsQueryHandler 
        : IRequestHandler<GetAllLocationsQueryRequest, GetAllLocationsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllLocationsQueryResponse> Handle(GetAllLocationsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<Location, bool>>? filter = null;
            if (request.WarehouseId.HasValue)
            {
                filter = l => l.WarehouseId == request.WarehouseId.Value;
            }

            var locations = await _unitOfWork.Repositories<Location>()
                .GetAll(filter, l => l.ChildLocations);

            var dtos = locations.Select(l => new GetLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                WarehouseId = l.WarehouseId,
                ParentId = l.ParentId,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                ChildLocations = l.ChildLocations?.Select(c => new GetLocationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    WarehouseId = c.WarehouseId,
                    ParentId = c.ParentId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList()
            }).ToList();

            return new GetAllLocationsQueryResponse { Locations = dtos };
        }
    }
}