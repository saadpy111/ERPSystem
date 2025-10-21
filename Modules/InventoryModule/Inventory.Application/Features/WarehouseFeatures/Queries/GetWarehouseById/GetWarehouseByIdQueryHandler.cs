using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;

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

            var dto =  new GetWarehouseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.LocationDetails
            };

            return new GetWarehouseByIdQueryResponse()
            {
                WarehouseDto = dto
            };
        }
    }
}