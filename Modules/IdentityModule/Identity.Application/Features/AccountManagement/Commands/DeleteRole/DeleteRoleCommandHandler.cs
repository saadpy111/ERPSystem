using Identity.Application.Contracts.Persistence;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoleCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteRoleResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
            if (role == null)
                return new DeleteRoleResponse { Success = false, Error = "Role not found in this tenant." };
            
            // Scope validation
            if (role.Scope != request.Scope)
                return new DeleteRoleResponse { Success = false, Error = "Unauthorized access to this role scope." };

            var result = await _authRepository.DeleteRoleAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new DeleteRoleResponse { Success = false, Error = errors };
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new DeleteRoleResponse { Success = true };
        }
    }
}
