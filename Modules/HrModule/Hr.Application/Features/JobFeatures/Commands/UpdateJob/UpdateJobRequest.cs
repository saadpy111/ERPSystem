using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.JobFeatures.UpdateJob
{
    public class UpdateJobRequest : IRequest<UpdateJobResponse>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public WorkType? WorkType { get; set; }
        public DateTime PublishedDate { get; set; }
        public JobStatus Status { get; set; }
        public string? Responsibilities { get; set; }
        public string? RequiredSkills { get; set; }
        public string? RequiredExperience { get; set; }
        public string? RequiredQualification { get; set; }
    }
}