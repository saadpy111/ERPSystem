using MediatR;

namespace Hr.Application.Features.JobFeatures.DeactivateJob
{
    public class DeactivateJobRequest : IRequest<DeactivateJobResponse>
    {
        public int JobId { get; set; }
    }
}
