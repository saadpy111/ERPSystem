using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetAllWarehouses
{
    public class GetAllWarehousesQueryHandler : IRequestHandler<GetAllWarehousesQueryRequest, GetAllWarehousesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllWarehousesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllWarehousesQueryResponse> Handle(GetAllWarehousesQueryRequest request, CancellationToken cancellationToken)
        {
            var warehouses = await _unitOfWork.Repositories<Warehouse>().GetAll();
            var dtos =  warehouses.Select(w => new GetWarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Location = w.LocationDetails
            }).ToList();
            return new GetAllWarehousesQueryResponse()
            {
                WarehouseDtos = dtos
            };
        }
    }
}