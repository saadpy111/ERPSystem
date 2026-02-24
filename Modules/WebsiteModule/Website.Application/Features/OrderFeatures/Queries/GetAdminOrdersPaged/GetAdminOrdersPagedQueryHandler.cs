using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminOrdersPaged
{
    public class GetAdminOrdersPagedQueryHandler : IRequestHandler<GetAdminOrdersPagedQuery, PagedResult<AdminOrderListDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAdminOrdersPagedQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResult<AdminOrderListDto>> Handle(
            GetAdminOrdersPagedQuery request, 
            CancellationToken cancellationToken)
        {
            return await _orderRepository.GetAdminOrdersPagedAsync(
                request.Filter, 
                
                cancellationToken);
        }
    }
}
