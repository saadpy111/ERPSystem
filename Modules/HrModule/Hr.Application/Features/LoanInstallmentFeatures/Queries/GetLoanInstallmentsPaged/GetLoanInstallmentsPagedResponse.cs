using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged
{
    public class GetLoanInstallmentsPagedResponse
    {
        public PagedResult<LoanInstallmentDto> PagedResult { get; set; } = new PagedResult<LoanInstallmentDto>();
    }
}
