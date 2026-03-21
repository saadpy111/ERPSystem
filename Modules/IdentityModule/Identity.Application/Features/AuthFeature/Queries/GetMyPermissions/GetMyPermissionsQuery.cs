using MediatR;

namespace Identity.Application.Features.AuthFeature.Queries.GetMyPermissions
{
    /// <summary>
    /// Query to fetch the current authenticated user's effective permissions from the DB.
    /// Claims in the userId and tenantId are extracted from the JWT by the caller (controller)
    /// and passed in explicitly — keeping the handler infrastructure-free.
    /// </summary>
    public class GetMyPermissionsQuery : IRequest<GetMyPermissionsResponse>
    {
        /// <summary>User ID extracted from the "sub" / NameIdentifier claim.</summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>Tenant ID extracted from the "tenant" claim.</summary>
        public string TenantId { get; init; } = string.Empty;
    }
}
