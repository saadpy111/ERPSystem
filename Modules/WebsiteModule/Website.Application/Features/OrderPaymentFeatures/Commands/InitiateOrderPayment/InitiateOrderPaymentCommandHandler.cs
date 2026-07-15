using MediatR;
using SharedKernel.Multitenancy;
using SharedKernel.Website;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.OrderPaymentFeatures.Commands.InitiateOrderPayment
{
    public class InitiateOrderPaymentCommandHandler
        : IRequestHandler<InitiateOrderPaymentCommand, InitiateOrderPaymentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IOrderPaymentService _orderPaymentService;

        public InitiateOrderPaymentCommandHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider,
            IOrderPaymentService orderPaymentService)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _orderPaymentService = orderPaymentService;
        }

        public async Task<InitiateOrderPaymentResponse> Handle(
            InitiateOrderPaymentCommand command,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new InitiateOrderPaymentResponse
                {
                    Success = false,
                    Error = "Tenant not resolved."
                };
            }

            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.GetByIdAsync(command.OrderId);

            if (order == null)
            {
                return new InitiateOrderPaymentResponse
                {
                    Success = false,
                    Error = "Order not found."
                };
            }

            if (order.UserId != command.UserId)
            {
                return new InitiateOrderPaymentResponse
                {
                    Success = false,
                    Error = "Order does not belong to the current user."
                };
            }

            if ( order.Status != OrderStatus.Pending)
            {
                return new InitiateOrderPaymentResponse
                {
                    Success = false,
                    Error = $"Order cannot be paid. Current status: {order.Status}"
                };
            }

            var amountCents = (long)Math.Round(order.TotalAmount * 100m, MidpointRounding.AwayFromZero);

            var request = new InitiateOrderPaymentRequest(
                UserId: command.UserId,
                TenantId: tenantId,
                OrderId: order.Id.ToString(),
                AmountCents: amountCents,
                CurrencyCode: "EGP",
                CustomerFirstName: order.CustomerName,
                CustomerLastName: "client",
                CustomerEmail: "",
                CustomerPhone: order.CustomerPhone);

            var result = await _orderPaymentService.InitiateOrderPaymentAsync(request, cancellationToken);

            if (result.Success && result.PaymentId != null)
            {
                order.PaymentId = result.PaymentId;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new InitiateOrderPaymentResponse
            {
                Success = result.Success,
                Error = result.Error,
                PaymentId = result.PaymentId,
                CheckoutUrl = result.CheckoutUrl,
                ClientSecret = result.ClientSecret,
                PublicKey = result.PublicKey,
                IsReused = result.IsReused
            };
        }
    }
}
