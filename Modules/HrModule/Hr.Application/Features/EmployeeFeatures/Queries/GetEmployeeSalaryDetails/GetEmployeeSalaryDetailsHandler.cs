using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeSalaryDetails
{
    public class GetEmployeeSalaryDetailsHandler : IRequestHandler<GetEmployeeSalaryDetailsRequest, GetEmployeeSalaryDetailsResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollRecordRepository _payrollRecordRepository;

        public GetEmployeeSalaryDetailsHandler(
            IEmployeeRepository employeeRepository,
            IPayrollRecordRepository payrollRecordRepository)
        {
            _employeeRepository = employeeRepository;
            _payrollRecordRepository = payrollRecordRepository;
        }

        public async Task<GetEmployeeSalaryDetailsResponse> Handle(GetEmployeeSalaryDetailsRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                return new GetEmployeeSalaryDetailsResponse
                {
                    EmployeeId = request.EmployeeId,
                    FullName = string.Empty,
                    PayrollRecords = new List<PayrollRecordDto>()
                };
            }

            var payrollRecords = await _payrollRecordRepository.GetByEmployeeIdAsync(request.EmployeeId);

            var payrollDtos = payrollRecords
                .OrderByDescending(p => p.PeriodYear).ThenByDescending(p => p.PeriodMonth)
                .Select(p => new PayrollRecordDto
                {
                    PayrollId = p.PayrollId,
                    EmployeeId = p.EmployeeId,
                    PeriodYear = p.PeriodYear,
                    PeriodMonth = p.PeriodMonth,
                    BaseSalary = p.BaseSalary,
                    TotalAllowances = p.TotalAllowances,
                    TotalDeductions = p.TotalDeductions,
                    TotalGrossSalary = p.TotalGrossSalary,
                    NetSalary = p.NetSalary,
                    Status = p.Status
                });

            return new GetEmployeeSalaryDetailsResponse
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                PayrollRecords = payrollDtos
            };
        }
    }
}
