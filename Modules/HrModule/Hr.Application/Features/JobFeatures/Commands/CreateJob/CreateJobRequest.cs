using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.JobFeatures.CreateJob
{
    public class CreateJobRequest : IRequest<CreateJobResponse>
    {
        public string Title { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public WorkType? WorkType { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
