using Hr.Application.Pagination;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStagesPaged
{
    public class GetRecruitmentStagesPagedResponse
    {
        public PagedResult<object> PagedResult { get; set; } = new PagedResult<object>();
    }
}
