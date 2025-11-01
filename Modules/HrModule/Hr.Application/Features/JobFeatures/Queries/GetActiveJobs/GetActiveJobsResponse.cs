using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.GetActiveJobs
{
    public class GetActiveJobsResponse
    {
        public IEnumerable<JobDto> Jobs { get; set; } = new List<JobDto>();
    }
}
