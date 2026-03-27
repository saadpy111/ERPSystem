using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.AssignRoleToUser;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommand : IRequest<UserRoleResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
    }

    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, UserRoleResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRoleFromUserCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRoleResponse> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindByIdAsync(request.UserId);
            if (user == null || user.TenantId != request.TenantId)
                return new UserRoleResponse { Success = false, Error = "User not found in this tenant." };

            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new UserRoleResponse { Success = false, Error = "Role not found in this tenant." };
            
            // Safety Rule: Validate UserType against Role.Scope
            if ((int)user.UserType != (int)role.Scope)
                return new UserRoleResponse { Success = false, Error = $"Cannot manage {role.Scope} role for {user.UserType} user." };

            var result = await _authRepository.RemoveRoleFromUserAsync(user, role, request.TenantId);
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
