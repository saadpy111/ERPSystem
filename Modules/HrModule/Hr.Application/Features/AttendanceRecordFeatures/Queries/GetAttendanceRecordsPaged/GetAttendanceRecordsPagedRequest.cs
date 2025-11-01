using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordsPaged
{
    public class GetAttendanceRecordsPagedRequest : IRequest<GetAttendanceRecordsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "Date";
        public bool IsDescending { get; set; } = false;
        public int? EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
