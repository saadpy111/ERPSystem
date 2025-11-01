using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.GetJobsByDepartment
{
    public class GetJobsByDepartmentResponse
    {
        public IEnumerable<JobDto> Jobs { get; set; } = new List<JobDto>();
    }
}
