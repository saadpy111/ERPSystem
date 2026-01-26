using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Commands.AddProductToOffer
{
    public class AddProductToOfferCommandHandler : IRequestHandler<AddProductToOfferCommandRequest, AddProductToOfferCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public AddProductToOfferCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<AddProductToOfferCommandResponse> Handle(AddProductToOfferCommandRequest request, CancellationToken cancellationToken)
        {
            var offerRepo = _unitOfWork.Repository<Offer>();
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var offerProductRepo = _unitOfWork.Repository<OfferProduct>();

            // Validate offer exists
            var offer = await offerRepo.GetByIdAsync(request.OfferId);
            if (offer == null)
            {
                return new AddProductToOfferCommandResponse
                {
                    Success = false,
                    Message = "Offer not found."
                };
            }

            // Validate product exists
            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new AddProductToOfferCommandResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            // Check if product already in offer
            var existing = await offerProductRepo.GetFirstAsync(op => op.OfferId == request.OfferId && op.ProductId == request.ProductId);
            if (existing != null)
            {
                return new AddProductToOfferCommandResponse
                {
                    Success = false,
                    Message = "Product is already in this offer."
                };
            }

            // Add product to offer
            var offerProduct = new OfferProduct
            {
                OfferId = request.OfferId,
                ProductId = request.ProductId,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await offerProductRepo.AddAsync(offerProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddProductToOfferCommandResponse { Success = true };
        }
    }
}
