using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeSalaryDetails
{
    public class GetEmployeeSalaryDetailsResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }
        public IEnumerable<PayrollRecordDto> PayrollRecords { get; set; } = new List<PayrollRecordDto>();
    }
}
