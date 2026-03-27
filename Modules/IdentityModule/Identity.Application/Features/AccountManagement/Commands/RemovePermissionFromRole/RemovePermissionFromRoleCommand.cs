using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleCommand : IRequest<RolePermissionResponse>
    {
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
        /// <summary>Permission name or ID (route param from controller).</summary>
        public string PermissionId { get; set; } = string.Empty;
    }

    public class RemovePermissionFromRoleCommandHandler
        : IRequestHandler<RemovePermissionFromRoleCommand, RolePermissionResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRolePermissionRepository _rolePermRepo;
        private readonly IUnitOfWork _unitOfWork;

        public RemovePermissionFromRoleCommandHandler(
            IAuthRepository authRepository,
            IRolePermissionRepository rolePermRepo,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _rolePermRepo   = rolePermRepo;
            _unitOfWork     = unitOfWork;
        }

        public async Task<RolePermissionResponse> Handle(
            RemovePermissionFromRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new RolePermissionResponse { Success = false, Error = "Role not found in this tenant." };
            
            // Scope validation
            if (role.Scope != request.Scope)
                return new RolePermissionResponse { Success = false, Error = "Unauthorized access to this role scope." };

            var allPermissions = await _rolePermRepo.GetAllPermissionsAsync();
            var permission = allPermissions.FirstOrDefault(p =>
                p.Id == request.PermissionId || p.Name == request.PermissionId);
            if (permission == null)
                return new RolePermissionResponse { Success = false, Error = "Permission not found." };

            if (!await _rolePermRepo.RoleHasPermissionAsync(role.Id, permission.Id, request.TenantId))
                return new RolePermissionResponse { Success = false, Error = "Role does not have this permission." };

            await _rolePermRepo.RemovePermissionFromRoleAsync(role.Id, permission.Id, request.TenantId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new RolePermissionResponse { Success = true };
        }
    }
}
