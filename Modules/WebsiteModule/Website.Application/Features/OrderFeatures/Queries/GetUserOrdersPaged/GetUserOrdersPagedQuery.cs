using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrdersPaged
{
    public class GetUserOrdersPagedQuery : IRequest<PagedResult<UserOrderListDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
