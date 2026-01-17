using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.TenantFeature.Commands.LeaveCompany
{
    public class LeaveCompanyCommandHandler : IRequestHandler<LeaveCompanyCommand, LeaveCompanyResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveCompanyCommandHandler(
            IAuthRepository authRepository,
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveCompanyResponse> Handle(LeaveCompanyCommand request, CancellationToken cancellationToken)
        {
            // Validate user exists and can leave
            var user = await _authRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new LeaveCompanyResponse
                {
                    Success = false,
                    Error = "User not found."
                };
            }

            if (user.TenantId == null)
            {
                return new LeaveCompanyResponse
                {
                    Success = false,
                    Error = "User is not associated with any company."
                };
            }

            // Owners cannot leave - they must transfer ownership first
            if (user.State == UserTenantState.TenantOwner)
            {
                return new LeaveCompanyResponse
                {
                    Success = false,
                    Error = "Company owners cannot leave. Please transfer ownership first."
                };
            }

            if (user.State != UserTenantState.TenantMember)
            {
                return new LeaveCompanyResponse
                {
                    Success = false,
                    Error = "User cannot leave company."
                };
            }

            var currentTenantId = user.TenantId;

            // ATOMIC TRANSACTION BEGINS

            // Step 1: Remove all role assignments
            await _authRepository.RemoveUserRolesAsync(user.Id, currentTenantId);

            // Step 2: Remove all direct permission assignments
            await _authRepository.RemoveUserPermissionsAsync(user.Id, currentTenantId);

            // Step 3: Update User - Remove Tenant
            user.TenantId = null;
            user.State = UserTenantState.PendingTenant; // Back to pre-tenant state
            user.TenantJoinedAt = null;
            await _authRepository.UpdateAsync(user);

            // SINGLE SAVE POINT - Atomic Commit
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Step 4: Generate NEW JWT Token WITHOUT Tenant Context
            var newToken = _jwtTokenService.GenerateToken(user, new List<string>(), new List<string>(), null);

            return new LeaveCompanyResponse
            {
                Success = true,
                NewToken = newToken
            };
        }
    }
}
