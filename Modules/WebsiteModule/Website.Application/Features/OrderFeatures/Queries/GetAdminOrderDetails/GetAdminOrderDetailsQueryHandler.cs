using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminOrderDetails
{
    public class GetAdminOrderDetailsQueryHandler : IRequestHandler<GetAdminOrderDetailsQuery, OrderDetailsDto?>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAdminOrderDetailsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDetailsDto?> Handle(
            GetAdminOrderDetailsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _orderRepository.GetAdminOrderDetailsAsync(
                request.OrderId, 
              
                cancellationToken);
        }
    }
}
