using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommand : IRequest<UserRoleResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string AssignedBy { get; set; } = string.Empty;
    }

    public class UserRoleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, UserRoleResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRoleToUserCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRoleResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            // Validate user belongs to tenant
            var user = await _authRepository.FindByIdAsync(request.UserId);
            if (user == null || user.TenantId != request.TenantId)
                return new UserRoleResponse { Success = false, Error = "User not found in this tenant." };

            // Validate role belongs to tenant
            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new UserRoleResponse { Success = false, Error = "Role not found in this tenant." };
            
            // Safety Rule: Validate UserType against Role.Scope
            if ((int)user.UserType != (int)role.Scope)
                return new UserRoleResponse { Success = false, Error = $"Cannot assign {role.Scope} role to {user.UserType} user." };

            var result = await _authRepository.AssignRoleToUserAsync(user, role, request.TenantId, request.AssignedBy);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new UserRoleResponse { Success = false, Error = errors };
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new UserRoleResponse { Success = true };
        }
    }
}
