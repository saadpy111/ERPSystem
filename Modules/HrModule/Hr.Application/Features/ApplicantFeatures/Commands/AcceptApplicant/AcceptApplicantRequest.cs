using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.AcceptApplicant
{
    public class AcceptApplicantRequest : IRequest<AcceptApplicantResponse>
    {
        public int ApplicantId { get; set; }
        public string? Notes { get; set; }
    }
}
