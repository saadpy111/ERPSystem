using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OfferFeatures.Queries.GetOfferById
{
    public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQueryRequest, GetOfferByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfferByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetOfferByIdQueryResponse> Handle(GetOfferByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(request.Id, o => o.OfferProducts);

            if (offer == null)
            {
                return new GetOfferByIdQueryResponse { Offer = null };
            }

            var dto = new OfferDetailDto
            {
                Id = offer.Id,
                Name = offer.Name,
                Description = offer.Description,
                DiscountType = offer.DiscountType,
                DiscountValue = offer.DiscountValue,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.IsActive,
                Products = offer.OfferProducts.Select(op => new OfferProductDto
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product?.NameSnapshot ?? string.Empty
                }).ToList()
            };

            return new GetOfferByIdQueryResponse { Offer = dto };
        }
    }
}
