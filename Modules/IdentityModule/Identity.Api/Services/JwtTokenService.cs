using Identity.Application.Contracts.Services;
using Identity.Application.Settings;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Services
{

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;
        public JwtTokenService(IOptions<JwtSettings> options) => _settings = options.Value;

        public string GenerateToken(ApplicationUser user, IList<string> roles, IList<string> permissions, string? tenantId)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim("fullName", user.FullName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("state", user.State.ToString()) // Add state claim
        };

            // Only add tenant-specific claims if user has a tenant
            if (!string.IsNullOrEmpty(tenantId))
            {
                claims.Add(new Claim("tenant", tenantId)); // Add tenant claim
                
                // Add role claims
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
                
                // Add permission claims
                claims.AddRange(permissions.Select(p => new Claim("permission", p)));
            }
            // Pre-tenant users (TenantId = NULL) get minimal claims: userId, email, state only

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
