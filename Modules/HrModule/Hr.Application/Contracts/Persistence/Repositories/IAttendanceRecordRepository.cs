using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IAttendanceRecordRepository
    {
        Task<AttendanceRecord> AddAsync(AttendanceRecord attendanceRecord);
        Task<AttendanceRecord?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();
        Task<IEnumerable<AttendanceRecord>> GetByEmployeeIdAsync(int employeeId);
        Task<PagedResult<AttendanceRecord>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, DateTime? startDate = null, DateTime? endDate = null, string? orderBy = null, bool isDescending = false);
        void Update(AttendanceRecord attendanceRecord);
        void Delete(AttendanceRecord attendanceRecord);
    }
}