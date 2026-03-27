using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole
{
    public class AddPermissionToRoleCommandHandler
        : IRequestHandler<AddPermissionToRoleCommand, RolePermissionResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRolePermissionRepository _rolePermRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AddPermissionToRoleCommandHandler(
            IAuthRepository authRepository,
            IRolePermissionRepository rolePermRepo,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _rolePermRepo   = rolePermRepo;
            _unitOfWork     = unitOfWork;
        }

        public async Task<RolePermissionResponse> Handle(
            AddPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            // Verify role belongs to tenant
            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new RolePermissionResponse { Success = false, Error = "Role not found in this tenant." };

            // Scope validation
            if (role.Scope != request.Scope)
                return new RolePermissionResponse { Success = false, Error = "Unauthorized access to this role scope." };

            // Verify permission exists globally
            var allPermissions = await _rolePermRepo.GetAllPermissionsAsync();
            var permission = allPermissions.FirstOrDefault(p =>
                p.Id == request.PermissionId || p.Name == request.PermissionId);
            if (permission == null)
                return new RolePermissionResponse { Success = false, Error = "Permission not found." };

            // Idempotency check
            if (await _rolePermRepo.RoleHasPermissionAsync(role.Id, permission.Id, request.TenantId))
                return new RolePermissionResponse { Success = false, Error = "Role already has this permission." };

            await _rolePermRepo.AddPermissionToRoleAsync(role.Id, permission.Id, request.TenantId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new RolePermissionResponse { Success = true };
        }
    }
}
