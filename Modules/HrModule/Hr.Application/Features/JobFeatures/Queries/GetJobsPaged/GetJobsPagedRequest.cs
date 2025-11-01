using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobsPaged
{
    public class GetJobsPagedRequest : IRequest<GetJobsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "Title";
        public bool IsDescending { get; set; } = false;
        public int? DepartmentId { get; set; }
        public string? Status { get; set; }
        public string? WorkType { get; set; }
    }
}
