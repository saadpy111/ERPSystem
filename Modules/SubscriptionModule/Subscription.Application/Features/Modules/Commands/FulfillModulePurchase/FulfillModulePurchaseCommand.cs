using MediatR;
using SharedKernel.Enums;

namespace Subscription.Application.Features.Modules.Commands.FulfillModulePurchase
{
    public class FulfillModulePurchaseCommand : IRequest<FulfillModulePurchaseResponse>
    {
        public string TenantId { get; }
        public string ModuleId { get; }
        public string CurrencyCode { get; }
        public BillingInterval Interval { get; }

        public FulfillModulePurchaseCommand(string tenantId, string moduleId, string currencyCode, BillingInterval interval)
        {
            TenantId = tenantId;
            ModuleId = moduleId;
            CurrencyCode = currencyCode;
            Interval = interval;
        }
    }

    public class FulfillModulePurchaseResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? SubscriptionId { get; set; }
    }
}
