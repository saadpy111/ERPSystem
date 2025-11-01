using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.UpdateEmployee
{
    public class UpdateEmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public EmployeeDto? Employee { get; set; }
    }
}
