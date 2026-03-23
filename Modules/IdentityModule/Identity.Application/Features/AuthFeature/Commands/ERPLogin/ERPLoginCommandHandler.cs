using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Application.Features.AuthFeature.Queries.Login;
using MediatR;
using SharedKernel.Multitenancy;
using Identity.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Commands.ERPLogin
{
    public class ERPLoginCommandHandler : IRequestHandler<ERPLoginCommandRequest, ERPLoginCommandResponse>
    {
        private readonly IAuthRepository _authService;
        private readonly IJwtTokenService _jwt;
        private readonly IPermissionRepository _permissionRepository;

        public ERPLoginCommandHandler(
            IAuthRepository authService,
            IJwtTokenService jwt,
            IPermissionRepository permissionRepository,
            ITenantProvider tenantProvider)
        {
            _authService = authService;
            _jwt = jwt;
            _permissionRepository = permissionRepository;
        }

        public async Task<ERPLoginCommandResponse> Handle(ERPLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _authService.FindByEmailAsync(request.LoginDto.Email);
            if (user == null)
                return new ERPLoginCommandResponse { Success = false, Error = "Invalid credentials" };

            var passwordValid = await _authService.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                return new ERPLoginCommandResponse { Success = false, Error = "Invalid credentials" };

            var roles = await _authService.GetUserRolesAsync(user);

            // The token is purposefully kept lightweight without permission claims.
            // Backend authorization will rely on the UI Database fallback logic (PermissionService).
            var tokenPermissions = new List<string>();
            var token = _jwt.GenerateToken(user, roles, tokenPermissions, user.TenantId);

            // Fetch actual permissions for frontend UI usage (show/hide pages/buttons).
            var tenantIdForPermissions = user.TenantId ?? string.Empty;
            var uiPermissions = string.IsNullOrEmpty(tenantIdForPermissions)
                ? new List<string>()
                : await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id, tenantIdForPermissions);

            return new ERPLoginCommandResponse
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
