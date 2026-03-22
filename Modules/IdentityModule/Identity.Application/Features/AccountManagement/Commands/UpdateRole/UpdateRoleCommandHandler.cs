using Identity.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Constants;

namespace Identity.Application.Features.AccountManagement.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _authRepository.GetRoleByIdAsync(request.RoleId, request.TenantId);
                if (role == null)
                    return new UpdateRoleResponse { Success = false, Error = "Role not found in this tenant." };
                if (role.Name == Roles.SuperAdmin)
                    return new UpdateRoleResponse { Success = false, Error = "Can not change SuperAdmin role" };
                role.Name = request.Name.Trim();
                var result = await _authRepository.UpdateRoleAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new UpdateRoleResponse { Success = false, Error = errors };
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new UpdateRoleResponse { Success = true };
            }
            catch(Exception)
            {
                return new UpdateRoleResponse { Success = false, Error = "Error while updating" };

            }
        }
    }
}
