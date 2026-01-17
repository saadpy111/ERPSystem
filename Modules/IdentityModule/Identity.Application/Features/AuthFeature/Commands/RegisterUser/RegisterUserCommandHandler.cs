using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Constants;
using SharedKernel.Constants.Permissions;

namespace Identity.Application.Features.AuthFeature.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IJwtTokenService _jwt;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
               IJwtTokenService jwt,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
             _jwt = jwt;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Error = "Email address is already registered."
                };
            }

            // Create user WITHOUT tenant (Phase 1 - Pre-Tenant)
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Email,
                NormalizedUserName = request.Email.ToUpper(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                EmailConfirmed = false, // Should be confirmed via email
                FullName = request.FullName,
                
                // Pre-tenant state
                TenantId = null, // NO TENANT
                State = UserTenantState.PendingTenant,
                TenantJoinedAt = null
            };


            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Error = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            var token = _jwt.GenerateToken(user, new List<string>(), new List<string>(), null);
            // TODO: Send email confirmation

            return new RegisterUserResponse
            {
                Success = true,
                Message = "User registered successfully. Please check your email to confirm your account.",
                UserId = user.Id,
                Token = token
            };
        }
    }
}
