
using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using MediatR;
using SharedKernel.Multitenancy;

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
            var permissions = await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id);
            var token = _jwt.GenerateToken(user, roles, permissions, user.TenantId);

            return new LoginQueryResponse { Success = true, Token = token };
        }
    }
}
