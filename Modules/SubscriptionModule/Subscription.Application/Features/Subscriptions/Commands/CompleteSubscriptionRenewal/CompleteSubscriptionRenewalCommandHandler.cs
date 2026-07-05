using MediatR;
using Microsoft.Extensions.Logging;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Subscriptions.Commands.CompleteSubscriptionRenewal
{
    public class CompleteSubscriptionRenewalCommandHandler : IRequestHandler<CompleteSubscriptionRenewalCommand, CompleteSubscriptionRenewalResponse>
    {
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompleteSubscriptionRenewalCommandHandler> _logger;

        public CompleteSubscriptionRenewalCommandHandler(
            ITenantSubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork,
            ILogger<CompleteSubscriptionRenewalCommandHandler> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CompleteSubscriptionRenewalResponse> Handle(CompleteSubscriptionRenewalCommand request, CancellationToken cancellationToken)
        {
            // 1. Load TenantSubscription by SubscriptionId
            var subscription = await _subscriptionRepository.GetByTenantIdAsync(request.TenantId);
            if (subscription == null || subscription.Id != request.SubscriptionId)
            {
                _logger.LogWarning("Subscription {SubscriptionId} not found or mismatch for Tenant {TenantId}.", request.SubscriptionId, request.TenantId);
                return new CompleteSubscriptionRenewalResponse { Success = false, Error = "Subscription not found." };
            }

            // 2. Guard: verify Status is Active or Suspended
            if (subscription.Status != SubscriptionStatus.Active && subscription.Status != SubscriptionStatus.Suspended)
            {
                _logger.LogWarning("Subscription {SubscriptionId} in status {Status} cannot be renewed.", subscription.Id, subscription.Status);
                return new CompleteSubscriptionRenewalResponse { Success = false, Error = $"Subscription in status {subscription.Status} cannot be renewed." };
            }

            var oldStatus = subscription.Status;
            var now = DateTime.UtcNow;

            // 3. Calculate new CurrentPeriodEnd from BillingCycle
            var periodEnd = subscription.BillingCycle.ToLowerInvariant() switch
            {
                "yearly" => subscription.CurrentPeriodEnd.AddYears(1),
                "quarterly" => subscription.CurrentPeriodEnd.AddMonths(3),
                "monthly" => subscription.CurrentPeriodEnd.AddMonths(1),
                _ => subscription.CurrentPeriodEnd.AddMonths(1)
            };

            // 4. Update dates
            subscription.CurrentPeriodStart = subscription.CurrentPeriodEnd;
            subscription.CurrentPeriodEnd = periodEnd;
            subscription.UpdatedAt = now;

            // 5. Flip Suspended status back to Active
            if (subscription.Status == SubscriptionStatus.Suspended)
            {
                subscription.Status = SubscriptionStatus.Active;
            }

            // 6. Create SubscriptionHistory record
            var history = new SubscriptionHistory
            {
                TenantSubscriptionId = subscription.Id,
                EventType = SubscriptionEventType.Renewed,
                FromPlanId = subscription.PlanId,
                ToPlanId = subscription.PlanId,
                FromStatus = oldStatus.ToString(),
                ToStatus = subscription.Status.ToString(),
                Notes = "Subscription renewed via payment callback.",
                CreatedAt = now
            };

            subscription.History.Add(history);

            await _subscriptionRepository.UpdateAsync(subscription);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Subscription {SubscriptionId} successfully renewed to {NewEnd}.", subscription.Id, periodEnd);

            return new CompleteSubscriptionRenewalResponse { Success = true };
        }
    }
}
