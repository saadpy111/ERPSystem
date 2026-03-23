
using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using MediatR;
using SharedKernel.Multitenancy;
using Identity.Domain.Extensions;

namespace Identity.Application.Features.AuthFeature.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
    {
        private readonly IAuthRepository _authService;
        private readonly IJwtTokenService _jwt;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ITenantProvider _tenantProvider;

        public LoginQueryHandler(
            IAuthRepository authService, 
            IJwtTokenService jwt, 
            IPermissionRepository permissionRepository,
            ITenantProvider tenantProvider)
        {
            _authService = authService;
            _jwt = jwt;
            _permissionRepository = permissionRepository;
            _tenantProvider = tenantProvider;
        }

        public async Task<LoginQueryResponse> Handle(LoginQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _authService.FindByEmailAsync(request.LoginDto.Email);
            if (user == null)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var passwordValid = await _authService.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var resolvedTenant = _tenantProvider.GetTenantId();

            //if (string.IsNullOrEmpty(resolvedTenant))
            //    return new LoginQueryResponse { Success = false, Error = "Tenant context is missing. Please use your company-specific URL." };

            if (user.TenantId != resolvedTenant)
                return new LoginQueryResponse { Success = false, Error = "User does not belong to this company." };

            var roles = await _authService.GetUserRolesAsync(user);
            
    
            var tokenPermissions = new List<string>();
            var token = _jwt.GenerateToken(user, roles, tokenPermissions, user.TenantId);

            // Fetch actual permissions for frontend UI usage (show/hide pages/buttons).
            var tenantIdForPermissions = user.TenantId ?? resolvedTenant ?? string.Empty;
            var uiPermissions = string.IsNullOrEmpty(tenantIdForPermissions)
                ? new List<string>()
                : await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id, tenantIdForPermissions);

            return new LoginQueryResponse 
            { 
                Success = true, 
                Token = token,
                UserId = user.Id,
                Roles = roles.Select(r => r.ToCleanRoleName(user.TenantId)).ToList(),
                Permissions = uiPermissions
            };
        }
    }
}
