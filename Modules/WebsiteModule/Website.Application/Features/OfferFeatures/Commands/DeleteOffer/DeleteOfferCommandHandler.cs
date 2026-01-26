using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Commands.DeleteOffer
{
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommandRequest, DeleteOfferCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteOfferCommandResponse> Handle(DeleteOfferCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(request.Id);

            if (offer == null)
            {
                return new DeleteOfferCommandResponse
                {
                    Success = false,
                    Message = "Offer not found."
                };
            }

            repo.Remove(offer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteOfferCommandResponse { Success = true };
        }
    }
}
