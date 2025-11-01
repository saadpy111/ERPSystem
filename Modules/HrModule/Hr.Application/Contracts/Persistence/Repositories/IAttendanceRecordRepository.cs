using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IAttendanceRecordRepository
    {
        Task<AttendanceRecord> AddAsync(AttendanceRecord attendanceRecord);
        Task<AttendanceRecord?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();
        Task<IEnumerable<AttendanceRecord>> GetByEmployeeIdAsync(int employeeId);
        void Update(AttendanceRecord attendanceRecord);
        void Delete(AttendanceRecord attendanceRecord);
    }
}
