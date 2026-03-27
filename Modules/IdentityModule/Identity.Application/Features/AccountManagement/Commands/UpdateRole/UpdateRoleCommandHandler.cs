using Identity.Application.Contracts.Persistence;
using Identity.Application.Features.AccountManagement.Commands.CreateRole;
using MediatR;
using SharedKernel.Constants;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                    return new UpdateRoleResponse { Success = false, Error = "Role not found" };

                // Scope validation
                if (role.Scope != request.Scope)
                    return new UpdateRoleResponse { Success = false, Error = "Unauthorized access to this role scope." };


                if (role.Name == $"{Roles.SuperAdmin}_{request.TenantId}")
                    return new UpdateRoleResponse { Success = false, Error = "Can not change SuperAdmin role" };


                var baseRoleName = request.Name.Trim();
                var roleName = $"{baseRoleName}_{request.TenantId}";
                if (await _authRepository.RoleExistsAsync(roleName))
                    return new UpdateRoleResponse { Success = false, Error = $"Role '{baseRoleName}' already exists." };

                role.Name = $"{request.Name.Trim()}_{request.TenantId}";
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
