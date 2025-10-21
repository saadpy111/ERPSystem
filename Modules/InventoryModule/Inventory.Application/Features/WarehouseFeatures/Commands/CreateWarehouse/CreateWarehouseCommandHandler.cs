using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.WarehouseDtos;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommandRequest, CreateWarehouseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateWarehouseCommandResponse> Handle(CreateWarehouseCommandRequest request, CancellationToken cancellationToken)
        {

            try
            {
                var entity = new Warehouse
                {
                    Name = request.Warehouse.Name,
                    LocationDetails = request.Warehouse.LocationDetails
                };
                await _unitOfWork.Repositories<Warehouse>().Add(entity);
                await _unitOfWork.CompleteAsync();

                return new CreateWarehouseCommandResponse
                {
                    Success = true,
                    Message = "?? ?????"
                };
            }
            catch
            {
                return new CreateWarehouseCommandResponse
                {
                    Success = true,
                    Message = " ??? ??? ??"

                };


            }
        }
    }
}