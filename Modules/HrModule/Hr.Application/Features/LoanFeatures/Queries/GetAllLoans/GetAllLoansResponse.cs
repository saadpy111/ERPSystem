using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanFeatures.GetAllLoans
{
    public class GetAllLoansResponse
    {
        public IEnumerable<LoanDto> Loans { get; set; } = new List<LoanDto>();
    }
}
