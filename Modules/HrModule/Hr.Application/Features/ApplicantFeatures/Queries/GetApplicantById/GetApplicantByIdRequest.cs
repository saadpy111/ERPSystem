using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantById
{
    public class GetApplicantByIdRequest : IRequest<GetApplicantByIdResponse>
    {
        public int Id { get; set; }
    }
}
