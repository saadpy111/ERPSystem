using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetAllLoanInstallments
{
    public class GetAllLoanInstallmentsResponse
    {
        public IEnumerable<LoanInstallmentDto> LoanInstallments { get; set; } = new List<LoanInstallmentDto>();
    }
}
