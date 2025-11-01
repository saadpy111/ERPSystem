using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.GetAllDepartments
{
    public class GetAllDepartmentsResponse
    {
        public List<DepartmentDto> Departments { get; set; } = new();
    }
}
