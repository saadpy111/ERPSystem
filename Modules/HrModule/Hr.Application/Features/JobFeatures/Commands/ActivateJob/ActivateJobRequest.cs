using MediatR;

namespace Hr.Application.Features.JobFeatures.ActivateJob
{
    public class ActivateJobRequest : IRequest<ActivateJobResponse>
    {
        public int JobId { get; set; }
    }
}
