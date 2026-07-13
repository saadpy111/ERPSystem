using MediatR;

namespace Subscription.Application.Features.Subscriptions.Queries.GetTenantWebsiteDetails
{
    public class GetTenantWebsiteDetailsQuery : IRequest<GetTenantWebsiteDetailsResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }
}
