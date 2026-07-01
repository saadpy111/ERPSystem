using MediatR;

namespace Subscription.Application.Features.Modules.Queries.GetEffectiveModules
{
    public class GetEffectiveModulesQuery : IRequest<GetEffectiveModulesResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }
}
