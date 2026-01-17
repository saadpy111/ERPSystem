using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommandRequest, CreateOfferCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public CreateOfferCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<CreateOfferCommandResponse> Handle(CreateOfferCommandRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();

            var offer = new Offer
            {
                Name = request.Name,
                Description = request.Description,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await repo.AddAsync(offer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateOfferCommandResponse
            {
                Success = true,
                OfferId = offer.Id
            };
        }
    }
}
