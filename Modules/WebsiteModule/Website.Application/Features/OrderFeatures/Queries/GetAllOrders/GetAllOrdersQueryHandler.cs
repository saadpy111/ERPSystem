using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OrderFeatures.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler
        : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(
            GetAllOrdersQueryRequest request,
            CancellationToken cancellationToken)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            // NOTE:
            // Global Query Filter handles Tenant automatically

            var orders = await orderRepo.GetAllAsync(
                request.Status.HasValue
                    ? o => o.Status == request.Status.Value
                    : null,
                o => o.Items);

            var result = orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new AdminOrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,

                    SubTotal = o.SubTotal,
                    DiscountTotal = o.DiscountTotal,
                    TotalAmount = o.TotalAmount,

                    ItemCount = o.Items.Count,
                    OrderDate = o.OrderDate,
                    UserId = o.UserId
                })
                .ToList();

            return new GetAllOrdersQueryResponse
            {
                Orders = result
            };
        }
    }
}
