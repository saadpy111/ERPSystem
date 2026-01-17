namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister
{
    public class ClientRegisterResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        
        // User info
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        
        // Tenant info
        public string? TenantId { get; set; }
        public string? StoreName { get; set; }
        
        // Authentication
        public string? Token { get; set; }
    }
}
