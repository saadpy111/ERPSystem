using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.CouponFeatures.Queries.GetCouponsPaged
{
    public class GetCouponsPagedQueryRequest : IRequest<PagedResult<CouponListItemDto>>
    {
        public CouponFilter Filter { get; set; } = new();
    }
}
