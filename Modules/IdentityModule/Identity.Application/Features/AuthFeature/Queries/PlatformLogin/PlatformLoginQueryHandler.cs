using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Application.Features.AuthFeature.Queries.Login;
using Identity.Domain.Extensions;
using MediatR;
using SharedKernel.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Queries.PlatformLogin
{
    public class PlatformLoginQueryHandler : IRequestHandler<PlatformLoginQueryRequest, PlatformLoginQueryResponse>
    {
        private readonly IAuthRepository _authService;
        private readonly IJwtTokenService _jwt;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ITenantProvider _tenantProvider;

        public PlatformLoginQueryHandler(
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

        public async Task<PlatformLoginQueryResponse> Handle(PlatformLoginQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _authService.FindByEmailAsync(request.LoginDto.Email);
            if (user == null)
                return new PlatformLoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var passwordValid = await _authService.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                return new PlatformLoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var resolvedTenant = _tenantProvider.GetTenantId();

            var roles = await _authService.GetUserRolesAsync(user);


            var tokenPermissions = new List<string>();
            var token = _jwt.GenerateToken(user, roles, tokenPermissions, user.TenantId);

            // Fetch actual permissions for frontend UI usage (show/hide pages/buttons).
            var tenantIdForPermissions = user.TenantId ?? resolvedTenant ?? string.Empty;
            var uiPermissions = string.IsNullOrEmpty(tenantIdForPermissions)
                ? new List<string>()
                : await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id, tenantIdForPermissions);

            return new PlatformLoginQueryResponse
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
