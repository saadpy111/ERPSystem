using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using Website.Domain.Entities;
using SharedKernel.Multitenancy;
using System.Linq.Expressions;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOffers
{
    public class GetStorefrontOffersQueryHandler : IRequestHandler<GetStorefrontOffersQueryRequest, GetStorefrontOffersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetStorefrontOffersQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetStorefrontOffersQueryResponse> Handle(GetStorefrontOffersQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var now = DateTime.UtcNow;

            Expression<Func<Offer, bool>> filter = o => 
                o.IsActive && 
                o.StartDate <= now && 
                o.EndDate >= now;

            var offers = await repo.SearchAsync(
                filter: filter,
                page: request.PageNumber,
                pageSize: request.PageSize,
                orderBy: q => q.OrderBy(o => o.EndDate));

            var dtos = offers.Items.Select(o => new StorefrontOfferDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                DiscountType = o.DiscountType,
                DiscountValue = o.DiscountValue,
                StartDate = o.StartDate,
                EndDate = o.EndDate
            }).ToList();

            var pagedResult = new PagedResult<StorefrontOfferDto>
            {
                Items = dtos,
                TotalCount = offers.TotalCount,
                Page = offers.Page,
                PageSize = offers.PageSize
            };

            return new GetStorefrontOffersQueryResponse { Result = pagedResult };
        }
    }
}
