using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordsPaged
{
    public class GetPayrollRecordsPagedResponse
    {
        public PagedResult<PayrollRecordDto> PagedResult { get; set; } = new PagedResult<PayrollRecordDto>();
    }
}
