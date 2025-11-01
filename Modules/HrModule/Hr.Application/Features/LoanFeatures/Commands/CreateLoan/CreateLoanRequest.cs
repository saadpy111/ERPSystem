using MediatR;

namespace Hr.Application.Features.LoanFeatures.CreateLoan
{
    public class CreateLoanRequest : IRequest<CreateLoanResponse>
    {
        public int EmployeeId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
    }
}
