using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollComponentFeatures.GetAllPayrollComponents
{
    public class GetAllPayrollComponentsResponse
    {
        public IEnumerable<PayrollComponentDto> PayrollComponents { get; set; } = new List<PayrollComponentDto>();
    }
}
