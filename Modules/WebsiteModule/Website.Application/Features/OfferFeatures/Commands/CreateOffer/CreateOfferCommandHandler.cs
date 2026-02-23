using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

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
            try
            {


                var tenantId = _tenantProvider.GetTenantId();

                var offer = new Offer
                {
                    Name = request.Name,
                    Description = request.Description,
                    DiscountType = request.DiscountType,
                    DiscountValue = request.DiscountValue,
                    Priority = request.Priority,
                    ScopeType = request.ScopeType,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsActive = request.IsActive,
                    TenantId = tenantId
                };

                // Advanced: Prevent overlapping offers with same ScopeType + Priority + overlapping dates
                var overlappingOfferId = await _unitOfWork.Repository<Offer>().AnyAsync(o =>
                    o.ScopeType == request.ScopeType &&
                    o.Priority == request.Priority &&
                    o.IsActive &&
                    o.StartDate < request.EndDate &&
                    request.StartDate < o.EndDate &&
                    o.TenantId == tenantId);

                if (overlappingOfferId)
                {
                    return new CreateOfferCommandResponse
                    {
                        Success = false,
                        Message = "An active offer with the same Scope, Priority, and overlapping dates already exists."
                    };
                }

                await _unitOfWork.Repository<Offer>().AddAsync(offer);

  

                // Handle scope-specific associations
                if (request.ScopeType == OfferScopeType.Product && request.ProductIds != null)
                {
                    foreach (var productId in request.ProductIds)
                    {
                        await _unitOfWork.Repository<OfferProduct>().AddAsync(new OfferProduct
                        {
                            OfferId = offer.Id,
                            ProductId = productId,
                            TenantId = tenantId
                        });
                    }
                }
                else if (request.ScopeType == OfferScopeType.Category && request.CategoryIds != null)
                {
                    foreach (var categoryId in request.CategoryIds)
                    {
                        await _unitOfWork.Repository<OfferCategory>().AddAsync(new OfferCategory
                        {
                            OfferId = offer.Id,
                            CategoryId = categoryId,
                            TenantId = tenantId
                        });
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new CreateOfferCommandResponse
                {
                    Success = true,
                    OfferId = offer.Id
                };
            }
            catch(Exception)
            {
                return new CreateOfferCommandResponse
                {
                    Success = false,
                    Message = "data invalid"
                };
            }
        }
    }
}
