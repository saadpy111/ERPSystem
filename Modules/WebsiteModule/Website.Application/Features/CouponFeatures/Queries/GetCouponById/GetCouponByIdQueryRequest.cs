using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.CouponFeatures.Queries.GetCouponById
{
    public class GetCouponByIdQueryRequest : IRequest<CouponDetailsDto?>
    {
        public Guid Id { get; set; }
    }
}
