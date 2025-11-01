using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.PromoteEmployee
{
    public class PromoteEmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public EmployeeDto? Employee { get; set; }
    }
}
