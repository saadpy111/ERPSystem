using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStageById
{
    public class GetRecruitmentStageByIdRequest : IRequest<GetRecruitmentStageByIdResponse>
    {
        public int Id { get; set; }
    }
}
