using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminOrdersPaged
{
    public class GetAdminOrdersPagedQuery : IRequest<PagedResult<AdminOrderListDto>>
    {
        public AdminOrderFilter Filter { get; set; } = new();
    }
}
