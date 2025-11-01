using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.ScheduleInterview
{
    public class ScheduleInterviewRequest : IRequest<ScheduleInterviewResponse>
    {
        public int ApplicantId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string? InterviewNotes { get; set; }
    }
}
