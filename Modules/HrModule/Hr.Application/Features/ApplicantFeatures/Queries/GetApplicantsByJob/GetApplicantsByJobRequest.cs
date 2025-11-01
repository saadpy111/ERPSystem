using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByJob
{
    public class GetApplicantsByJobRequest : IRequest<GetApplicantsByJobResponse>
    {
        public int JobId { get; set; }
    }
}
