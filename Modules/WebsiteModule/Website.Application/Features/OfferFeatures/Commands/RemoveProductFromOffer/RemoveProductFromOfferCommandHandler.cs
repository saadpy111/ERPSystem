using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Commands.RemoveProductFromOffer
{
    public class RemoveProductFromOfferCommandHandler : IRequestHandler<RemoveProductFromOfferCommandRequest, RemoveProductFromOfferCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductFromOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveProductFromOfferCommandResponse> Handle(RemoveProductFromOfferCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<OfferProduct>();
            var offerProduct = await repo.GetFirstAsync(op => op.OfferId == request.OfferId && op.ProductId == request.ProductId);

            if (offerProduct == null)
            {
                return new RemoveProductFromOfferCommandResponse
                {
                    Success = false,
                    Message = "Product not found in offer."
                };
            }

            repo.Remove(offerProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RemoveProductFromOfferCommandResponse { Success = true };
        }
    }
}
