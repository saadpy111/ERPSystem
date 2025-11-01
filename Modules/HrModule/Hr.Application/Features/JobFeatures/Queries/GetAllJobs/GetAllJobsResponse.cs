using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.GetAllJobs
{
    public class GetAllJobsResponse
    {
        public IEnumerable<JobDto> Jobs { get; set; } = new List<JobDto>();
    }
}
