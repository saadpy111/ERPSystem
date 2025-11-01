using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsPaged
{
    public class GetApplicantsPagedResponse
    {
        public PagedResult<ApplicantDto> PagedResult { get; set; } = new PagedResult<ApplicantDto>();
    }
}
