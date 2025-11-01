using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetActiveEmployees
{
    public class GetActiveEmployeesResponse
    {
        public IEnumerable<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
