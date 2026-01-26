using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.RemoveProductFromCollection
{
    public class RemoveProductFromCollectionCommandHandler : IRequestHandler<RemoveProductFromCollectionCommandRequest, RemoveProductFromCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductFromCollectionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveProductFromCollectionCommandResponse> Handle(RemoveProductFromCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var itemRepo = _unitOfWork.Repository<ProductCollectionItem>();
            var item = await itemRepo.GetFirstAsync(i => i.CollectionId == request.CollectionId && i.ProductId == request.ProductId);

            if (item == null)
            {
                return new RemoveProductFromCollectionCommandResponse
                {
                    Success = false,
                    Message = "Product not found in collection."
                };
            }

            itemRepo.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RemoveProductFromCollectionCommandResponse { Success = true };
        }
    }
}
