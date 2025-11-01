using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        private readonly HrDbContext _context;

        public AttendanceRecordRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceRecord> AddAsync(AttendanceRecord attendanceRecord)
        {
            await _context.AttendanceRecords.AddAsync(attendanceRecord);
            return attendanceRecord;
        }

        public async Task<AttendanceRecord?> GetByIdAsync(int id)
        {
            return await _context.AttendanceRecords
                .Include(ar => ar.Employee)
                .FirstOrDefaultAsync(ar => ar.RecordId == id);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync()
        {
            return await _context.AttendanceRecords
                .Include(ar => ar.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceRecord>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.AttendanceRecords
                .Where(ar => ar.EmployeeId == employeeId)
                .Include(ar => ar.Employee)
                .ToListAsync();
        }

        public void Update(AttendanceRecord attendanceRecord)
        {
            _context.AttendanceRecords.Update(attendanceRecord);
        }

        public void Delete(AttendanceRecord attendanceRecord)
        {
            _context.AttendanceRecords.Remove(attendanceRecord);
        }
    }
}
