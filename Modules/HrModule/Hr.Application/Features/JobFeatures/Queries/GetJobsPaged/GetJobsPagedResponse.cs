using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.GetJobsPaged
{
    public class GetJobsPagedResponse
    {
        public PagedResult<JobDto> PagedResult { get; set; } = new PagedResult<JobDto>();
    }
}
