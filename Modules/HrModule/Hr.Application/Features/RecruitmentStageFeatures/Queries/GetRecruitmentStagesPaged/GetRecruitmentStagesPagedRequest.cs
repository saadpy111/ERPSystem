using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStagesPaged
{
    public class GetRecruitmentStagesPagedRequest : IRequest<GetRecruitmentStagesPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "SequenceOrder";
        public bool IsDescending { get; set; } = false;
    }
}
