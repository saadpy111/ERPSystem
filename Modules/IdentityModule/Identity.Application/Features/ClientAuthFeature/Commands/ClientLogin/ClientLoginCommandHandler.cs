using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Website;

namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientLogin
{
    /// <summary>
    /// Handler for client (storefront customer) login.
    /// 
    /// Flow:
    /// 1. Resolve tenant by domain (via WebsiteModule)
    /// 2. Find user by email AND tenant (clients are tenant-scoped)
    /// 3. Validate credentials
    /// 4. Verify user is a Client type
    /// 5. Generate JWT token
    /// </summary>
    public class ClientLoginCommandHandler : IRequestHandler<ClientLoginCommand, ClientLoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITenantDomainResolver _tenantDomainResolver;
        private readonly IJwtTokenService _jwtTokenService;

        public ClientLoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITenantDomainResolver tenantDomainResolver,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tenantDomainResolver = tenantDomainResolver;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ClientLoginResponse> Handle(ClientLoginCommand request, CancellationToken cancellationToken)
        {
            // ===== STEP 1: Resolve tenant by domain =====
            var tenantResult = await _tenantDomainResolver.GetTenantByDomainAsync(request.Domain);
            
            if (tenantResult == null)
                return Fail("Store not found. Please check the domain.");

            // ===== STEP 2: Find user by email =====
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
                return Fail("Invalid email or password.");

            // ===== STEP 3: Verify user belongs to this tenant =====
            if (user.TenantId != tenantResult.TenantId)
                return Fail("Invalid email or password.");  // Generic error for security

            // ===== STEP 4: Verify user is a Client =====
            if (user.UserType != UserType.Client)
                return Fail("Invalid email or password.");  // Don't expose that it's a system user

            // ===== STEP 5: Validate password =====
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return Fail("Account is locked. Please try again later.");
                
                return Fail("Invalid email or password.");
            }

            // ===== STEP 6: Generate JWT token =====
            var roles = new List<string>();  // Clients typically don't have roles
            var permissions = new List<string>();  // Clients have no admin permissions
            
            var token = _jwtTokenService.GenerateToken(user, roles, permissions, tenantResult.TenantId);

            return new ClientLoginResponse
            {
                Success = true,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TenantId = tenantResult.TenantId,
                StoreName = tenantResult.StoreName,
                Token = token
            };
        }

        private static ClientLoginResponse Fail(string error)
        {
            return new ClientLoginResponse { Success = false, Error = error };
        }
    }
}
