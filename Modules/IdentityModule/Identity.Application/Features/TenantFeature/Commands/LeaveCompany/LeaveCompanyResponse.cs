namespace Identity.Application.Features.TenantFeature.Commands.LeaveCompany
{
    public class LeaveCompanyResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? NewToken { get; set; } // JWT without tenant context
    }
}
