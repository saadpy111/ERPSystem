using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.GetDepartmentsPaged
{
    public class GetDepartmentsPagedResponse
    {
        public PagedResult<DepartmentDto> PagedResult { get; set; } = new PagedResult<DepartmentDto>();
    }
}
