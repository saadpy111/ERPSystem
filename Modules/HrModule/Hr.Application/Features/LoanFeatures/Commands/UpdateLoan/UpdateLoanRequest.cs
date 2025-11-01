using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.LoanFeatures.UpdateLoan
{
    public class UpdateLoanRequest : IRequest<UpdateLoanResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
        public LoanStatus Status { get; set; }
    }
}
