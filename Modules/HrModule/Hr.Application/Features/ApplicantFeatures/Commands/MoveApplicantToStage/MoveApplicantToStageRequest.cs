using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.MoveApplicantToStage
{
    public class MoveApplicantToStageRequest : IRequest<MoveApplicantToStageResponse>
    {
        public int ApplicantId { get; set; }
        public int NewStageId { get; set; }
        public string? Notes { get; set; }
        public ApplicantStatus  ApplicantStatus { get; set; }
    }
}
