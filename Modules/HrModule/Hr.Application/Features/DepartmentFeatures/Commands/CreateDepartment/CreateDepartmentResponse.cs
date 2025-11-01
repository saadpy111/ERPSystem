using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.CreateDepartment
{
    public class CreateDepartmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DepartmentDto? Department { get; set; }
    }
}
