using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest
{
    public class CreateLeaveRequestRequest : IRequest<CreateLeaveRequestResponse>
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string? Notes { get; set; }
    }
}