using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord
{
    public class CreatePayrollRecordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PayrollRecordDto? PayrollRecord { get; set; }
    }
}
