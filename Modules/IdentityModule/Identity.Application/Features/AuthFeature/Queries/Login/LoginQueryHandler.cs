
using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using MediatR;

namespace Identity.Application.Features.AuthFeature.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
    {
        private readonly IAuthRepository _authService;
        private readonly IJwtTokenService _jwt;
        private readonly IPermissionRepository _permissionRepository;

        public LoginQueryHandler(
            IAuthRepository authService, 
            IJwtTokenService jwt, 
            IPermissionRepository permissionRepository)
        {
            _authService = authService;
            _jwt = jwt;
            _permissionRepository = permissionRepository;
        }

        public async Task<LoginQueryResponse> Handle(LoginQueryRequest request, CancellationToken cancellationToken)
        {
            // Find user by email (uses IgnoreQueryFilters internally in repository)
            var user = await _authService.FindByEmailAsync(request.LoginDto.Email);
            
            if (user == null)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            // Verify password
            var passwordValid = await _authService.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            // Get user roles
            var roles = await _authService.GetUserRolesAsync(user);

            // Get user effective permissions (from roles + direct assignments)
            var permissions = await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id);

            // Get user's tenant
            var tenantId = user.TenantId;

            //if (string.IsNullOrEmpty(tenantId))
            //    return new LoginQueryResponse { Success = false, Error = "User is not associated with any tenant" };

            // Generate JWT with roles, permissions, and tenant
            var token = _jwt.GenerateToken(user, roles, permissions, tenantId);

            return new LoginQueryResponse { Success = true, Token = token };
        }
    }
}
