using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PagedResult<AttendanceRecord>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, DateTime? startDate = null, DateTime? endDate = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.AttendanceRecords
                .Include(ar => ar.Employee)
                .AsQueryable();

            // Apply employee filter
            if (employeeId.HasValue)
            {
                query = query.Where(ar => ar.EmployeeId == employeeId.Value);
            }

            // Apply date range filter
            if (startDate.HasValue)
            {
                query = query.Where(ar => ar.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(ar => ar.Date <= endDate.Value);
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AttendanceRecord>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<AttendanceRecord> ApplyOrdering(IQueryable<AttendanceRecord> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Date";

            query = orderBy.ToLower() switch
            {
                "date" => isDescending ? query.OrderByDescending(ar => ar.Date) : query.OrderBy(ar => ar.Date),
                "checkintime" => isDescending ? query.OrderByDescending(ar => ar.CheckInTime) : query.OrderBy(ar => ar.CheckInTime),
                "checkouttime" => isDescending ? query.OrderByDescending(ar => ar.CheckOutTime) : query.OrderBy(ar => ar.CheckOutTime),
                "delayminutes" => isDescending ? query.OrderByDescending(ar => ar.DelayMinutes) : query.OrderBy(ar => ar.DelayMinutes),
                _ => isDescending ? query.OrderByDescending(ar => ar.Date) : query.OrderBy(ar => ar.Date)
            };

            return query;
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