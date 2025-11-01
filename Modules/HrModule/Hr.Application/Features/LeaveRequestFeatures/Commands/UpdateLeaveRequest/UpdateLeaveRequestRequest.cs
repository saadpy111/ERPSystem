using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.LeaveRequestFeatures.UpdateLeaveRequest
{
    public class UpdateLeaveRequestRequest : IRequest<UpdateLeaveRequestResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public LeaveRequestStatus Status { get; set; }
    }
}
