using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeRequest : IRequest<UpdateLeaveTypeResponse>
    {
        public int Id { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Notes { get; set; }
        public LeaveTypeStatus Status { get; set; }
    }
}