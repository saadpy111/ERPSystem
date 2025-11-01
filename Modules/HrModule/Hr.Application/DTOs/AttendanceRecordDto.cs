namespace Hr.Application.DTOs
{
    public class AttendanceRecordDto
    {
        public int RecordId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int DelayMinutes { get; set; }
    }
}
