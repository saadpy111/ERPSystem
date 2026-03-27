using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.ReplaceRolePermissions
{
    public class ReplaceRolePermissionsCommand : IRequest<RolePermissionResponse>
    {
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
        /// <summary>Full replacement set — can be permission names or IDs.</summary>
        public List<string> PermissionIds { get; set; } = new();
    }

    public class ReplaceRolePermissionsCommandHandler
        : IRequestHandler<ReplaceRolePermissionsCommand, RolePermissionResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRolePermissionRepository _rolePermRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ReplaceRolePermissionsCommandHandler(
            IAuthRepository authRepository,
            IRolePermissionRepository rolePermRepo,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _rolePermRepo   = rolePermRepo;
            _unitOfWork     = unitOfWork;
        }

        public async Task<RolePermissionResponse> Handle(
            ReplaceRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new RolePermissionResponse { Success = false, Error = "Role not found in this tenant." };
            
            // Scope validation
            if (role.Scope != request.Scope)
                return new RolePermissionResponse { Success = false, Error = "Unauthorized access to this role scope." };

            // Resolve names → IDs (accept both formats)
            var allPermissions = await _rolePermRepo.GetAllPermissionsAsync();
            var resolvedIds = request.PermissionIds
                .Select(p => allPermissions.FirstOrDefault(ap => ap.Id == p || ap.Name == p)?.Id)
                .Where(id => id != null)
                .Cast<string>()
                .Distinct()
                .ToList();

            // Validate that all supplied permissions actually exist
            if (resolvedIds.Count != request.PermissionIds.Distinct().Count())
                return new RolePermissionResponse { Success = false, Error = "One or more permissions not found." };

            await _rolePermRepo.ReplaceRolePermissionsAsync(role.Id, resolvedIds, request.TenantId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new RolePermissionResponse { Success = true };
        }
    }
}
