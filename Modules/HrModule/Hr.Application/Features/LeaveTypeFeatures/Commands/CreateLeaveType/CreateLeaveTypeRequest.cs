using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.CreateLeaveType
{
    public class CreateLeaveTypeRequest : IRequest<CreateLeaveTypeResponse>
    {
        public string LeaveTypeName { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Notes { get; set; }
        public LeaveTypeStatus Status { get; set; } = LeaveTypeStatus.NoPaid;
    }
}