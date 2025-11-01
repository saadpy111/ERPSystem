using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeesPaged
{
    public class GetEmployeesPagedResponse
    {
        public PagedResult<EmployeeDto> PagedResult { get; set; } = new PagedResult<EmployeeDto>();
    }
}
