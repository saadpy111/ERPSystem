using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetPermissions
{
    public class GetPermissionsQuery : IRequest<GetPermissionsResponse>
    {
        /// <summary>
        /// Determines which permission set to return:
        /// System → all non-website permissions (ERP modules)
        /// Client → only website-specific permissions
        /// </summary>
        public UserType UserType { get; set; }
    }

    public class PermissionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class GetPermissionsResponse
    {
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
