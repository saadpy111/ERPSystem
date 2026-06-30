using Identity.Domain.Enums;

namespace Identity.Application.Services
{
    public class RoleMapping
    {
        public string RoleName { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
    }

    public interface IModuleRoleMappingService
    {
        bool TryGetRoleMapping(string moduleCode, out RoleMapping mapping);
        bool IsSuperAdmin(string cleanRoleName);
        string? GetModuleCode(string cleanRoleName);
        IReadOnlyDictionary<string, RoleMapping> GetAllMappings();
    }
}
