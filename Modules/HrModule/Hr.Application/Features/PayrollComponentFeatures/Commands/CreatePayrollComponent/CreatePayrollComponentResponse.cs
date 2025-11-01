using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent
{
    public class CreatePayrollComponentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PayrollComponentDto? PayrollComponent { get; set; }
    }
}
