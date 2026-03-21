using Identity.Application.Contracts.Persistence;
using MediatR;

namespace Identity.Application.Features.AuthFeature.Queries.GetMyPermissions
{
    /// <summary>
    /// Handles <see cref="GetMyPermissionsQuery"/>.
    ///
    /// Deliberately fetches permissions fresh from the database — not from JWT claims —
    /// so the frontend always gets an up-to-date view of what the user can do.
    /// Tenant isolation is enforced inside the repository via the tenantId filter.
    /// </summary>
    public class GetMyPermissionsQueryHandler
        : IRequestHandler<GetMyPermissionsQuery, GetMyPermissionsResponse>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetMyPermissionsQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<GetMyPermissionsResponse> Handle(
            GetMyPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            // Always fetch from DB (not from token) for accurate, real-time UI control.
            // The repository applies tenant isolation: WHERE UserId = @userId AND TenantId = @tenantId.
            var permissions = await _permissionRepository
                .GetUserEffectivePermissionsAsync(request.UserId, request.TenantId);

            return new GetMyPermissionsResponse
            {
                Permissions = permissions
            };
        }
    }
}
