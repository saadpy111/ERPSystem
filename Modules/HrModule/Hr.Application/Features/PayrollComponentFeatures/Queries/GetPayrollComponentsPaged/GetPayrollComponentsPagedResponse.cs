using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentsPaged
{
    public class GetPayrollComponentsPagedResponse
    {
        public PagedResult<PayrollComponentDto> PagedResult { get; set; } = new PagedResult<PayrollComponentDto>();
    }
}
