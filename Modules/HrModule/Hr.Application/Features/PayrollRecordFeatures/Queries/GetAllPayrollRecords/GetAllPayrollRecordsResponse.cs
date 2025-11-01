using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollRecordFeatures.GetAllPayrollRecords
{
    public class GetAllPayrollRecordsResponse
    {
        public IEnumerable<PayrollRecordDto> PayrollRecords { get; set; } = new List<PayrollRecordDto>();
    }
}
