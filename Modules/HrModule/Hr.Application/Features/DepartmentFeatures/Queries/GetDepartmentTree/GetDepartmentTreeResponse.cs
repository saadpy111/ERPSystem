using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.Queries.GetDepartmentTree
{
    public class GetDepartmentTreeResponse
    {
        public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    }
}