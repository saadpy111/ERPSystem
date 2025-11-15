using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class LoanDto
    {
        public int LoanId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? JobName { get; set; }
        public string? DepartmentName { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
        public decimal RemainingBalance { get; set; }
        public LoanStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}