using MediatR;
using SharedKernel.Enums;

namespace Subscription.Application.Features.Modules.Commands.PurchaseModule
{
    public class PurchaseModuleCommand : IRequest<PurchaseModuleResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "USD";
        public BillingInterval Interval { get; set; } = BillingInterval.Monthly;
    }

    public class PurchaseModuleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? SubscriptionId { get; set; }
    }
}
