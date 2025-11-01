using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByStage
{
    public class GetApplicantsByStageRequest : IRequest<GetApplicantsByStageResponse>
    {
        public int StageId { get; set; }
    }
}
