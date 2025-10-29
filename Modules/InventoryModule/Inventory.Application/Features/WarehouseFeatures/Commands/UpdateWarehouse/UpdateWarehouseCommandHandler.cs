using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommandRequest, UpdateWarehouseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateWarehouseCommandResponse> Handle(UpdateWarehouseCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Warehouse>();
                var entity = await repo.GetById(request.Warehouse.Id);
                if (entity == null) return null;

                entity.Name = request.Warehouse.Name;
                entity.LocationDetails = request.Warehouse.Location;
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