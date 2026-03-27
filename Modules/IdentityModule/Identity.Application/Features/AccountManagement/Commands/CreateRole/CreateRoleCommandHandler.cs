using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRolePermissionRepository _rolePermRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(
            IAuthRepository authRepository,
            IRolePermissionRepository rolePermRepo,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _rolePermRepo   = rolePermRepo;
            _unitOfWork     = unitOfWork;
        }

        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Role names are unique per tenant by convention: "Name_TenantCode"
            var baseRoleName = request.Name.Trim();
            var roleName = $"{baseRoleName}_{request.TenantId}";
            if (await _authRepository.RoleExistsAsync(roleName))
                return new CreateRoleResponse { Success = false, Error = $"Role '{baseRoleName}' already exists." };

            var role = new ApplicationRole
            {
                Id             = Guid.NewGuid().ToString(),
                Name           = roleName,
                NormalizedName = roleName.ToUpperInvariant(),
                TenantId       = request.TenantId,
                Scope          = request.Scope
            };

            var result = await _authRepository.CreateRoleAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new CreateRoleResponse { Success = false, Error = errors };
            }

            // Optionally seed initial permissions
            if (request.PermissionIds.Any())
                await _rolePermRepo.AssignPermissionsToRoleAsync(role.Id, request.PermissionIds, request.TenantId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new CreateRoleResponse { Success = true, RoleId = role.Id };
        }
    }
}
