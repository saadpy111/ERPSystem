using MediatR;
using Subscription.Application.DTOs;

namespace Subscription.Application.Features.Modules.Queries.GetPurchasedModules
{
    public class GetPurchasedModulesQuery : IRequest<GetPurchasedModulesResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }

    public class GetPurchasedModulesResponse
    {
        public bool Success { get; set; }
        public List<PurchasedModuleDto> Data { get; set; } = new();
    }
}
