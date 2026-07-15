using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.OrderPaymentFeatures.Queries.GetOrderPaymentStatus
{
    public class GetOrderPaymentStatusQueryHandler
        : IRequestHandler<GetOrderPaymentStatusQuery, GetOrderPaymentStatusResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderPaymentStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetOrderPaymentStatusResponse> Handle(
            GetOrderPaymentStatusQuery query,
            CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(query.OrderId);

            if (order == null)
            {
                return new GetOrderPaymentStatusResponse
                {
                    Success = false,
                    Error = "Order not found."
                };
            }

            if (order.UserId != query.UserId)
            {
                return new GetOrderPaymentStatusResponse
                {
                    Success = false,
                    Error = "Order does not belong to the current user."
                };
            }

            return new GetOrderPaymentStatusResponse
            {
                Success = true,
                OrderStatus = order.Status.ToString(),
                PaymentStatus = order.PaymentId != null ? "Initiated" : "NotInitiated",
                IsPaid = order.Status == OrderStatus.Paid
            };
        }
    }
}
