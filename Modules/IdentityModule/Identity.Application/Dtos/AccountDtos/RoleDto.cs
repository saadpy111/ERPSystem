namespace Identity.Application.Dtos.AccountDtos
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
