using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrdersPaged
{
    public class GetUserOrdersPagedQueryHandler : IRequestHandler<GetUserOrdersPagedQuery, PagedResult<UserOrderListDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetUserOrdersPagedQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResult<UserOrderListDto>> Handle(
            GetUserOrdersPagedQuery request, 
            CancellationToken cancellationToken)
        {
            return await _orderRepository.GetUserOrdersPagedAsync(
                request.UserId, 
                request.PageNumber, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
