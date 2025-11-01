using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.RejectApplicant
{
    public class RejectApplicantRequest : IRequest<RejectApplicantResponse>
    {
        public int ApplicantId { get; set; }
        public string? RejectionReason { get; set; }
    }
}
