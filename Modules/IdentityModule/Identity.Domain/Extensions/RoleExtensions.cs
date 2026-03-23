using System;

namespace Identity.Domain.Extensions
{
    public static class RoleExtensions
    {
        public static string ToCleanRoleName(this string roleName, string? tenantId)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(tenantId))
                return roleName;

            roleName = roleName.Trim();
            tenantId = tenantId.Trim();

            var suffix = $"_{tenantId}";

            if (!roleName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                return roleName;

            return roleName.Substring(0, roleName.Length - suffix.Length);
        }
    }
}
