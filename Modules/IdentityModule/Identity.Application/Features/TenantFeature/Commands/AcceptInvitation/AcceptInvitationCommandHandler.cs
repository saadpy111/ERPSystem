using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.TenantFeature.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, AcceptInvitationResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITenantInvitationRepository _invitationRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AcceptInvitationCommandHandler(
            IAuthRepository authRepository,
            ITenantInvitationRepository invitationRepository,
            IPermissionRepository permissionRepository,
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _invitationRepository = invitationRepository;
            _permissionRepository = permissionRepository;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AcceptInvitationResponse> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            // Validate user exists and is in PendingTenant state
            var user = await _authRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "User not found."
                };
            }

            if (user.TenantId != null)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "User already belongs to a company."
                };
            }

            if (user.State != UserTenantState.PendingTenant)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "User is not in pending tenant state."
                };
            }

            // Validate invitation
            var invitation = await _invitationRepository.GetByTokenAsync(request.InvitationToken);
            if (invitation == null)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "Invalid invitation token."
                };
            }

            if (invitation.Status != InvitationStatus.Pending)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "Invitation is no longer valid."
                };
            }

            if (invitation.ExpiresAt < DateTime.UtcNow)
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "Invitation has expired."
                };
            }

            // Validate email matches
            if (!invitation.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                return new AcceptInvitationResponse
                {
                    Success = false,
                    Error = "Invitation email does not match user email."
                };
            }

            // ATOMIC TRANSACTION BEGINS

            // Step 1: Update User - Assign to Tenant
            user.TenantId = invitation.TenantId;
            user.State = UserTenantState.TenantMember; // User becomes member
            user.TenantJoinedAt = DateTime.UtcNow;
            await _authRepository.UpdateAsync(user);

            // Step 2: Assign Role from Invitation
            var userRole = new ApplicationUserRole
            {
                UserId = user.Id,
                RoleId = invitation.RoleId,
                TenantId = invitation.TenantId,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = invitation.InvitedBy
            };
            await _authRepository.AddUserRoleAsync(userRole);

            // Step 3: Mark Invitation as Accepted
            invitation.Status = InvitationStatus.Accepted;
            invitation.AcceptedAt = DateTime.UtcNow;
            invitation.AcceptedBy = user.Id;
            await _invitationRepository.UpdateAsync(invitation);

            // SINGLE SAVE POINT - Atomic Commit
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Step 4: Get User Permissions (after successful commit)
            var permissions = await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id);

            // Step 5: Get User Roles
            var roles = await _userManager.GetRolesAsync(user);

            // Step 6: Generate NEW JWT Token with Tenant Context
            var newToken = _jwtTokenService.GenerateToken(user, roles.ToList(), permissions, invitation.TenantId);

            return new AcceptInvitationResponse
            {
                Success = true,
                TenantId = invitation.TenantId,
                TenantName = invitation.Tenant.Name,
                NewToken = newToken
            };
        }
    }
}
