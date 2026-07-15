using Microsoft.Extensions.Logging;
using SharedKernel.Multitenancy;
using SharedKernel.Website;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Services.Payment
{
    public class WebsiteOrderCompletionService : IWebsiteOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WebsiteOrderCompletionService> _logger;

        public WebsiteOrderCompletionService(
            IUnitOfWork unitOfWork,
            ILogger<WebsiteOrderCompletionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<WebsiteOrderCompletionResult> CompleteOrderPaymentAsync(
            CompleteOrderPaymentRequest request,
            CancellationToken cancellationToken)
        {
          
            var orderRepo = _unitOfWork.Repository<Order>();
            var walletRepo = _unitOfWork.Repository<Wallet>();
            var walletTxRepo = _unitOfWork.Repository<WalletTransaction>();

            var order = await orderRepo.GetByIdAsync(Guid.Parse(request.OrderId));

            if (order == null)
            {
                _logger.LogError("Order {OrderId} not found for payment completion.", request.OrderId);
                return new WebsiteOrderCompletionResult(false, "Order not found.");
            }

            if (order.Status == OrderStatus.Paid)
            {
                _logger.LogInformation("Order {OrderId} is already paid. Skipping.", request.OrderId);
                return new WebsiteOrderCompletionResult(true, null);
            }

            if (order.Status != OrderStatus.Pending)
            {
                _logger.LogWarning("Order {OrderId} has status {Status}. Cannot complete payment.", request.OrderId, order.Status);
                return new WebsiteOrderCompletionResult(false, $"Order status is {order.Status}. Cannot complete payment.");
            }

            // Mark order as paid
            order.Status = OrderStatus.Paid;
            order.PaidAt = DateTime.UtcNow;
            order.PaymentId = request.PaymentId;
            orderRepo.Update(order);

            // Find or create wallet for this tenant
            var wallet = (await walletRepo.GetAllAsync(w => w.TenantId == order.TenantId)).FirstOrDefault();
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    TenantId = order.TenantId,
                    CurrentBalance = 0m
                };
                await walletRepo.AddAsync(wallet);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var balanceBefore = wallet.CurrentBalance;
            wallet.CurrentBalance += order.TotalAmount;
            walletRepo.Update(wallet);

            var walletTx = new WalletTransaction
            {
                WalletId = wallet.Id,
                Type = WalletTransactionType.Credit,
                Amount = order.TotalAmount,
                BalanceBefore = balanceBefore,
                BalanceAfter = wallet.CurrentBalance,
                Reference = $"Order:{order.Id}",
                Description = $"Payment for Order {order.OrderNumber}",
                TenantId = order.TenantId
            };
            await walletTxRepo.AddAsync(walletTx);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Order {OrderId} marked as Paid. Wallet {WalletId} credited with {Amount}. New balance: {Balance}",
                order.Id, wallet.Id, order.TotalAmount, wallet.CurrentBalance);

            return new WebsiteOrderCompletionResult(true, null);
        }
    }
}
