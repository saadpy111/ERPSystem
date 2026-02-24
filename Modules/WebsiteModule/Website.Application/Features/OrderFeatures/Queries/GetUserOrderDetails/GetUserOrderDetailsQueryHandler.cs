using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrderDetails
{
    public class GetUserOrderDetailsQueryHandler : IRequestHandler<GetUserOrderDetailsQuery, UserOrderDetailsDto?>
    {
        private readonly IOrderRepository _orderRepository;

        public GetUserOrderDetailsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<UserOrderDetailsDto?> Handle(
            GetUserOrderDetailsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _orderRepository.GetUserOrderDetailsAsync(
                request.OrderId, 
                request.UserId, 
                cancellationToken);
        }
    }
}
