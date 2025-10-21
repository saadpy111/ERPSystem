using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.LocationFeatures.Commands.DeleteLocation
{
    public class DeleteLocationCommandHandler 
        : IRequestHandler<DeleteLocationCommandRequest, DeleteLocationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteLocationCommandResponse> Handle(DeleteLocationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repo = _unitOfWork.Repositories<Location>();
                var entity = await repo.GetById(request.Id);
                
                if (entity == null)
                    return new DeleteLocationCommandResponse { Success = false };

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();

                return new DeleteLocationCommandResponse { Success = true };
            }
            catch
            {
                return new DeleteLocationCommandResponse { Success = false };
            }
        }
    }
}