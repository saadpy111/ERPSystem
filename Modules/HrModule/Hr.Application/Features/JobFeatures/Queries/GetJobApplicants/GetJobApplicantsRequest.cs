using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobApplicants
{
    public class GetJobApplicantsRequest : IRequest<GetJobApplicantsResponse>
    {
        public int JobId { get; set; }
    }
}
