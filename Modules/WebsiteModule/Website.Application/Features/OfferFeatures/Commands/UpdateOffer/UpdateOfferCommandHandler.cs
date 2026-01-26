using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommandRequest, UpdateOfferCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateOfferCommandResponse> Handle(UpdateOfferCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(request.Id);

            if (offer == null)
            {
                return new UpdateOfferCommandResponse
                {
                    Success = false,
                    Message = "Offer not found."
                };
            }

            // Update only provided fields
            if (request.Name != null) offer.Name = request.Name;
            if (request.Description != null) offer.Description = request.Description;
            if (request.DiscountType.HasValue) offer.DiscountType = request.DiscountType.Value;
            if (request.DiscountValue.HasValue) offer.DiscountValue = request.DiscountValue.Value;
            if (request.StartDate.HasValue) offer.StartDate = request.StartDate.Value;
            if (request.EndDate.HasValue) offer.EndDate = request.EndDate.Value;
            if (request.IsActive.HasValue) offer.IsActive = request.IsActive.Value;
            offer.UpdatedAt = DateTime.UtcNow;

            repo.Update(offer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateOfferCommandResponse { Success = true };
        }
    }
}
