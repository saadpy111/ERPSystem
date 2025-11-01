using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.CreateJob
{
    public class CreateJobResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public JobDto? Job { get; set; }
    }
}
