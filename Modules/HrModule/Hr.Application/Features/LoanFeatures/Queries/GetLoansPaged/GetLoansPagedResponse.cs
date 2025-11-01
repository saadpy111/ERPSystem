using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanFeatures.GetLoansPaged
{
    public class GetLoansPagedResponse
    {
        public PagedResult<LoanDto> PagedResult { get; set; } = new PagedResult<LoanDto>();
    }
}
