using SharedKernel.Subscription;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums; // For SubscriptionStatus, SubscriptionEventType
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingInterval = SharedKernel.Enums.BillingInterval; // Alias for clarity

namespace Subscription.Application.Services
{
    /// <summary>
    /// Implements ISubscriptionService for creating subscriptions when tenants are created.
    /// Called by Identity module's CreateCompanyCommandHandler.
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IPlanModuleRepository _planModuleRepository;
        private readonly ITenantSubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(
            ISubscriptionPlanRepository planRepository,
            IPlanModuleRepository planModuleRepository,
            ITenantSubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork)
        {
            _planRepository = planRepository;
            _planModuleRepository = planModuleRepository;
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSubscriptionResult> CreateSubscriptionAsync(
            string tenantId,
            string tenantName,
            string planCodeOrId,
            string currencyCode,
            BillingInterval interval)
        {
            try
            {
                // 1. Get plan by Code or ID (case-insensitive)
                var plan = await GetPlanByCodeOrIdAsync(planCodeOrId);
                
                if (plan == null)
                {
                    return new CreateSubscriptionResult
                    {
                        Success = false,
                        Error = "Plan not found"
                    };
                }

                // 2. Validate plan is active
                if (!plan.IsActive)
                {
                    return new CreateSubscriptionResult
                    {
                        Success = false,
                        Error = "Plan is not available"
                    };
                }

                // 3. NULL SAFETY: Ensure pricing is loaded and configured
                if (plan.Prices == null || !plan.Prices.Any())
                {
                    return new CreateSubscriptionResult
                    {
                        Success = false,
                        Error = $"Plan '{plan.Code}' has no pricing configured. Please contact support."
                    };
                }

                // 4. NULL SAFETY: Check if any active prices exist
                if (!plan.Prices.Any(p => p.IsActive))
                {
                    return new CreateSubscriptionResult
                    {
                        Success = false,
                        Error = $"Plan '{plan.Code}' has no active pricing. Please contact support."
                    };
                }

                // 5. Validate currency AND interval are supported
                var price = plan.Prices.FirstOrDefault(p => 
                    p.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase) && 
                    p.Interval == interval &&  // BILLING INTERVAL FIX
                    p.IsActive);
                
                if (price == null)
                {
                    return new CreateSubscriptionResult
                    {
                        Success = false,
                        Error = $"Currency {currencyCode} with {interval} billing not supported for this plan"
                    };
                }

                // 6. BILLING INTERVAL FIX: Calculate period end based on selected interval
                var periodEnd = interval switch
                {
                    BillingInterval.Monthly => DateTime.UtcNow.AddMonths(1),
                    BillingInterval.Yearly => DateTime.UtcNow.AddYears(1),
                    BillingInterval.Quarterly => DateTime.UtcNow.AddMonths(3),
                    _ => DateTime.UtcNow.AddMonths(1) // Safe fallback
                };

                // 7. Determine subscription status (Trial vs Active)
                var status = plan.IsTrial ? SubscriptionStatus.Trial : SubscriptionStatus.Active;
                var trialEndsAt = plan.IsTrial ? DateTime.UtcNow.AddDays(plan.TrialDays) : (DateTime?)null;

                // 8. Create subscription with correct billing cycle
                var subscription = new TenantSubscription
                {
                    TenantId = tenantId,
                    PlanId = plan.Id,
                    Status = status,
                    StartDate = DateTime.UtcNow,
                    TrialEndsAt = trialEndsAt,
                    CurrentPeriodStart = DateTime.UtcNow,
                    CurrentPeriodEnd = trialEndsAt ?? periodEnd,  // Use trial end or interval-based period
                    BillingCycle = interval.ToString().ToLower(), // "monthly", "yearly", "quarterly"
                    BillingAnchorDay = DateTime.UtcNow.Day,
                    CurrencyCode = currencyCode.ToUpperInvariant(),
                    AutoRenew = !plan.IsTrial, // Trials don't auto-renew
                    LastQuotaResetAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _subscriptionRepository.CreateAsync(subscription);

                // 6. Create history entry
                var eventType = plan.IsTrial ? SubscriptionEventType.TrialStarted : SubscriptionEventType.Created;
                var history = new SubscriptionHistory
                {
                    TenantSubscriptionId = subscription.Id,
                    EventType = eventType,
                    ToPlanId = plan.Id,
                    ToStatus = status.ToString(),
                    Notes = $"{(plan.IsTrial ? "Trial" : "Active")} subscription created for {tenantName} - Plan: {plan.DisplayName}",
                    PerformedBy = "System",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.SaveChangesAsync();

                // 7. Get enabled modules
                var enabledModules = await _planModuleRepository.GetEnabledModulesAsync(plan.Id);

                return new CreateSubscriptionResult
                {
                    Success = true,
                    SubscriptionId = subscription.Id,
                    EnabledModules = enabledModules.Select(m => m.ModuleName).ToList(),
                    PlanCode = plan.Code,
                    PlanName = plan.DisplayName,
                    IsTrial = plan.IsTrial,
                    TrialEndsAt = trialEndsAt
                };
            }

            catch (Exception ex)
            {
                return new CreateSubscriptionResult
                {
                    Success = false,
                    Error = $"Failed to create subscription: {ex.Message}"
                };
            }
        }

        private async Task<SubscriptionPlan?> GetPlanByCodeOrIdAsync(string planCodeOrId)
        {
            // Try by Code first (case-insensitive)
            var plan = await _planRepository.GetByCodeAsync(planCodeOrId);
            
            if (plan == null)
            {
                // Try by ID
                plan = await _planRepository.GetByIdAsync(planCodeOrId);
            }

            return plan;
        }
    }
}
