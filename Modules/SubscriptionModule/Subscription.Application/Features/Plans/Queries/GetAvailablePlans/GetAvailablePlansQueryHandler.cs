using MediatR;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs;
using Subscription.Domain.Enums;
using SharedKernel.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Plans.Queries.GetAvailablePlans
{
    public class GetAvailablePlansQueryHandler : IRequestHandler<GetAvailablePlansQuery, GetAvailablePlansResponse>
    {
        private readonly ISubscriptionPlanRepository _planRepository;

        public GetAvailablePlansQueryHandler(ISubscriptionPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<GetAvailablePlansResponse> Handle(GetAvailablePlansQuery request, CancellationToken cancellationToken)
        {
            // Get all active plans
            var plans = await _planRepository.GetAllActivePlansAsync();

            // Filter by visibility
            if (!request.IncludeHidden)
            {
                plans = plans.Where(p => p.IsVisible).ToList();
            }

            // Map to DTOs
            var planDtos = plans.Select(p => new AvailablePlanDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DisplayName = p.DisplayName,
                Description = p.Description,
                Modules = p.PlanModules.Where(pm => pm.IsEnabled).Select(pm => pm.ModuleName).ToList(),
                HasTrial = p.IsTrial,
                TrialDays = p.TrialDays,
                Pricing = p.Prices
                    .Where(price => price.IsActive)
                    .Where(price => string.IsNullOrEmpty(request.CurrencyCode) || 
                                   price.CurrencyCode.Equals(request.CurrencyCode, StringComparison.OrdinalIgnoreCase))
                    .Select(price => new PlanPricingDto
                    {
                        Currency = price.CurrencyCode,
                        Amount = price.Amount,
                        Interval = price.Interval.ToString(),
                        Savings = GetSavingsText(price.Interval, price.Amount, p.Prices.ToList())
                    })
                    .ToList(),
                Limits = new PlanLimitsDto
                {
                    MaxUsers = p.MaxUsers,
                    MaxStorage = FormatStorage(p.MaxStorageBytes),
                    MaxProducts = p.MaxProducts,
                    MaxMonthlyTransactions = p.MaxMonthlyTransactions
                }
            }).ToList();

            return new GetAvailablePlansResponse
            {
                Success = true,
                Plans = planDtos
            };
        }

        private string FormatStorage(long bytes)
        {
            if (bytes == -1) return "Unlimited";
            if (bytes >= 1073741824) return $"{bytes / 1073741824} GB";
            if (bytes >= 1048576) return $"{bytes / 1048576} MB";
            return $"{bytes} bytes";
        }

        private string? GetSavingsText(BillingInterval interval, decimal amount, List<Domain.Entities.PlanPrice> allPrices)
        {
            if (interval == BillingInterval.Yearly)
            {
                var monthlyPrice = allPrices.FirstOrDefault(p => 
                    p.Interval == BillingInterval.Monthly && p.IsActive);
                
                if (monthlyPrice != null)
                {
                    var yearlyEquivalent = monthlyPrice.Amount * 12;
                    if (amount < yearlyEquivalent)
                    {
                        var monthsSaved = (int)Math.Floor((yearlyEquivalent - amount) / monthlyPrice.Amount);
                        if (monthsSaved > 0)
                            return $"{monthsSaved} month{(monthsSaved > 1 ? "s" : "")} free";
                    }
                }
            }
            return null;
        }
    }
}
