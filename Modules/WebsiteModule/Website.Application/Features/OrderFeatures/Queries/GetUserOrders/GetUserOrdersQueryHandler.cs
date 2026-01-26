using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQueryRequest, GetUserOrdersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUserOrdersQueryResponse> Handle(GetUserOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orders = await orderRepo.GetAllAsync(
                o => o.UserId == request.UserId,
                o => o.Items);

            var ordersDto = orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new UserOrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    ItemCount = o.Items.Count,
                    OrderDate = o.OrderDate
                })
                .ToList();

            return new GetUserOrdersQueryResponse { Orders = ordersDto };
        }
    }
}
