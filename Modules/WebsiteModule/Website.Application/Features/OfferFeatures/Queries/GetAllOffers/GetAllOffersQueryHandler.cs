using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Queries.GetAllOffers
{
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQueryRequest, GetAllOffersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOffersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllOffersQueryResponse> Handle(GetAllOffersQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offers = await repo.GetAllAsync(
                filter: request.IsActive.HasValue ? o => o.IsActive == request.IsActive : null,
                includes: o => o.OfferProducts);

            var dtos = offers.Select(o => new OfferDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                DiscountType = o.DiscountType,
                DiscountValue = o.DiscountValue,
                StartDate = o.StartDate,
                EndDate = o.EndDate,
                IsActive = o.IsActive,
                ProductCount = o.OfferProducts.Count
            }).ToList();

            return new GetAllOffersQueryResponse { Offers = dtos };
        }
    }
}
