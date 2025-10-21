using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.DeleteWarehouse
{
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommandRequest, DeleteWarehouseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteWarehouseCommandResponse> Handle(DeleteWarehouseCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Warehouse>();
                var entity = await repo.GetById(request.Id);
                if (entity == null)
                {
                    return new DeleteWarehouseCommandResponse
                    {
                        Success = true,
                        Message = "?? ?????"
                    };
                }

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();
                return new DeleteWarehouseCommandResponse
                {
                    Success = false,
                    Message = " ??? ??? ??"

                };
            }
            catch
            {
                return new DeleteWarehouseCommandResponse
                {
                    Success = true,
                    Message = " ??? ??? ??"

                };
            }
         
        }
    }
}