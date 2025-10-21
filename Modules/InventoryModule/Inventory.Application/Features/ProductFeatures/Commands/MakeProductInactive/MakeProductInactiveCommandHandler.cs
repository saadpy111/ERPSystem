using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProductFeatures.Commands.MakeProductInactive
{
    public class MakeProductInactiveCommandHandler : IRequestHandler<MakeProductInactiveCommandRequest, MakeProductInactiveCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MakeProductInactiveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MakeProductInactiveCommandResponse> Handle(MakeProductInactiveCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repositories<Product>();
            var entity = await repo.GetById(request.Id);
            if (entity == null)
                return new MakeProductInactiveCommandResponse { Success = false };

            entity.IsActive = false;
            repo.Update(entity);
            await _unitOfWork.CompleteAsync();
            return new MakeProductInactiveCommandResponse { Success = true };
        }
    }
}