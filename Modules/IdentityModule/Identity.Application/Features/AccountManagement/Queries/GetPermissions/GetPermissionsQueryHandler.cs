using Identity.Application.Contracts.Persistence;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetPermissions
{
    /// <summary>
    /// Returns the assignable permission catalog scoped to the caller's user segment.
    ///
    /// Filtering rule (applied in Application layer, not in the controller):
    ///   • UserType.Client  → only permissions whose Module is in WebsiteModules
    ///   • UserType.System  → all permissions whose Module is NOT in WebsiteModules
    ///
    /// This keeps the controller oblivious to domain filtering logic.
    /// </summary>
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, GetPermissionsResponse>
    {


        private readonly IRolePermissionRepository _rolePermRepo;

        public GetPermissionsQueryHandler(IRolePermissionRepository rolePermRepo)
            => _rolePermRepo = rolePermRepo;

        public async Task<GetPermissionsResponse> Handle(
            GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var all = await _rolePermRepo.GetAllPermissionsAsync();

            // Filter by scope
            var filtered = request.UserType == UserType.Client
                ? all.Where(p => p.Module == "Website")   // Client: website-only
                : all.Where(p => p.Module!="Website");  // System: everything else

            var dtos = filtered
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Name)
                .Select(p => new PermissionDto
                {
                    Id          = p.Id,
                    Name        = p.Name,
                    Module      = p.Module,
                    Description = p.Description
                })
                .ToList();

            return new GetPermissionsResponse { Permissions = dtos };
        }
    }
}
