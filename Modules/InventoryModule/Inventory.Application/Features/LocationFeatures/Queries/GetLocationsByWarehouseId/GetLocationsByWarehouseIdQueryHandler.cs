using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Features.LocationFeatures.Queries.GetLocationsByWarehouseId
{
    public class GetLocationsByWarehouseIdQueryHandler : IRequestHandler<GetLocationsByWarehouseIdQueryRequest, GetLocationsByWarehouseIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLocationsByWarehouseIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetLocationsByWarehouseIdQueryResponse> Handle(GetLocationsByWarehouseIdQueryRequest request, CancellationToken cancellationToken)
        {
            var locations = await _unitOfWork.Repositories<Location>()
                .GetAll(l => l.WarehouseId == request.WarehouseId);

            var dtos = locations.Select(l => new GetLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                WarehouseId = l.WarehouseId,
                ParentId = l.ParentId,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToList();

            return new GetLocationsByWarehouseIdQueryResponse
            {
                Locations = dtos
            };
        }
    }
}