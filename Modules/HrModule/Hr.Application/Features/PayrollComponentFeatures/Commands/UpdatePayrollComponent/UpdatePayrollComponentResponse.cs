using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent
{
    public class UpdatePayrollComponentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PayrollComponentDto? PayrollComponent { get; set; }
    }
}
