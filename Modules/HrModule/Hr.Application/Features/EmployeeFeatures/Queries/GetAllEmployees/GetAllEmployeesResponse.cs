using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetAllEmployees
{
    public class GetAllEmployeesResponse
    {
        public IEnumerable<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
