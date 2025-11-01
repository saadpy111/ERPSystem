using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.DeleteApplicant
{
    public class DeleteApplicantRequest : IRequest<DeleteApplicantResponse>
    {
        public int ApplicantId { get; set; }
    }
}
