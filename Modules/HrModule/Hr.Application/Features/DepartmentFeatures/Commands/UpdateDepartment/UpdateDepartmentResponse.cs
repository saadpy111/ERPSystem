using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.UpdateDepartment
{
    public class UpdateDepartmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DepartmentDto? Department { get; set; }
    }
}
