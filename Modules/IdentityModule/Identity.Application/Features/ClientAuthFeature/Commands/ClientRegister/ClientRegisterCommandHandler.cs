using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Multitenancy;
using SharedKernel.Website;

namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister
{
    /// <summary>
    /// Handler for client (storefront customer) registration.
    /// 
    /// Flow:
    /// 1. Resolve tenant by domain (via WebsiteModule)
    /// 2. Validate domain is active/published
    /// 3. Create user with UserType = Client
    /// 4. Generate JWT token
    /// </summary>
    public class ClientRegisterCommandHandler : IRequestHandler<ClientRegisterCommand, ClientRegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ITenantProvider _tenantProvider;

        public ClientRegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            ITenantDomainResolver tenantDomainResolver,
            IJwtTokenService jwtTokenService,
            ITenantProvider tenantProvider)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _tenantProvider = tenantProvider;
        }

        public async Task<ClientRegisterResponse> Handle(ClientRegisterCommand request, CancellationToken cancellationToken)
        {
            // ===== STEP 1: Resolve tenant by domain =====
            var tenantId =  _tenantProvider.GetTenantId();
            
            if (string.IsNullOrEmpty(tenantId))
                return  Fail("Store not found. Please check the domain.");


            // ===== STEP 2: Validate email is unique within tenant =====
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.TenantId == tenantId)
                return Fail("An account with this email already exists for this store.");

            // ===== STEP 3: Create client user =====
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.Phone,
                
                // Client-specific settings
                UserType = UserType.Client,
                TenantId = tenantId,
                State = UserTenantState.TenantMember,  // Clients are members of tenant
                TenantJoinedAt = DateTime.UtcNow,
                
                EmailConfirmed = true  // Auto-confirm for clients (can add email verification later)
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Fail($"Registration failed: {errors}");
            }

            // ===== STEP 4: Generate JWT token =====
            // Clients have minimal permissions - just basic access
            var roles = new List<string>();  // Clients typically don't have roles
            var permissions = new List<string>();  // Clients have no admin permissions
            
            var token = _jwtTokenService.GenerateToken(user, roles, permissions, tenantId);

            return new ClientRegisterResponse
            {
                Success = true,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TenantId = tenantId,
                Token = token
            };
        }

        private static ClientRegisterResponse Fail(string error)
        {
            return new ClientRegisterResponse { Success = false, Error = error };
        }
    }
}
