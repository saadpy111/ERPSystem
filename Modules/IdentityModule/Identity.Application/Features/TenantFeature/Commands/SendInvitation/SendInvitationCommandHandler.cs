using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.TenantFeature.Commands.SendInvitation
{
    public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, SendInvitationResponse>
    {
        private readonly ITenantInvitationRepository _invitationRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SendInvitationCommandHandler(
            ITenantInvitationRepository invitationRepository,
            ITenantRepository tenantRepository,
            IAuthRepository authRepository,
            IUnitOfWork unitOfWork)
        {
            _invitationRepository = invitationRepository;
            _tenantRepository = tenantRepository;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SendInvitationResponse> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            // Validate tenant exists
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null)
            {
                return new SendInvitationResponse
                {
                    Success = false,
                    Error = "Tenant not found."
                };
            }

            // Check if user already exists with this email and has a tenant
            var existingUser = await _authRepository.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.TenantId != null)
            {
                return new SendInvitationResponse
                {
                    Success = false,
                    Error = "User already belongs to a company."
                };
            }

            // Check for existing pending invitation
            var existingInvitations = await _invitationRepository.GetByEmailAsync(request.Email);
            var pendingInvitation = existingInvitations.FirstOrDefault(i => 
                i.TenantId == request.TenantId && 
                i.Status == InvitationStatus.Pending &&
                i.ExpiresAt > DateTime.UtcNow);

            if (pendingInvitation != null)
            {
                return new SendInvitationResponse
                {
                    Success = false,
                    Error = "A pending invitation already exists for this email."
                };
            }

            // Generate invitation
            var invitationToken = GenerateInvitationToken();
            var expiresAt = DateTime.UtcNow.AddDays(7); // 7 days expiry

            var invitation = new TenantInvitation
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = request.TenantId,
                Email = request.Email.ToLower(),
                RoleId = request.RoleId,
                InvitedBy = request.InvitedByUserId,
                Status = InvitationStatus.Pending,
                InvitationToken = invitationToken,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            await _invitationRepository.CreateAsync(invitation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Send invitation email via IInvitationService

            return new SendInvitationResponse
            {
                Success = true,
                InvitationToken = invitationToken,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateInvitationToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .TrimEnd('=');
        }
    }
}
