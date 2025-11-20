using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.Persistence.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly HrDbContext _context;

        public LeaveRequestRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
            return leaveRequest;
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .FirstOrDefaultAsync(lr => lr.RequestId == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.EmployeeId == employeeId)
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .ToListAsync();
        }

        public async Task<PagedResult<LeaveRequest>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, string? leaveType = null, string? status = null, string? searchTerm = null, string? orderBy = null, bool isDescending = false)
        {
            var query = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .AsQueryable();

            // Apply employee filter
            if (employeeId.HasValue)
            {
                query = query.Where(lr => lr.EmployeeId == employeeId.Value);
            }

            // Apply leave type filter
            if (!string.IsNullOrWhiteSpace(leaveType))
            {
                // Try to parse as int for LeaveTypeId
                if (int.TryParse(leaveType, out var leaveTypeId))
                {
                    query = query.Where(lr => lr.LeaveTypeId == leaveTypeId);
                }
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<LeaveRequestStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(lr => lr.Status == statusEnum);
                }
            }

            // Apply search term filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(lr => 
                    lr.Employee.FullName.ToLower().Contains(searchTermLower) ||
                    lr.LeaveType.LeaveTypeName.ToLower().Contains(searchTermLower) ||
                    lr.Notes.ToLower().Contains(searchTermLower));
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LeaveRequest>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<LeaveRequest> ApplyOrdering(IQueryable<LeaveRequest> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "StartDate";

            query = orderBy.ToLower() switch
            {
                "startdate" => isDescending ? query.OrderByDescending(lr => lr.StartDate) : query.OrderBy(lr => lr.StartDate),
                "enddate" => isDescending ? query.OrderByDescending(lr => lr.EndDate) : query.OrderBy(lr => lr.EndDate),
                "durationdays" => isDescending ? query.OrderByDescending(lr => lr.DurationDays) : query.OrderBy(lr => lr.DurationDays),
                "status" => isDescending ? query.OrderByDescending(lr => lr.Status) : query.OrderBy(lr => lr.Status),
                "leavetypename" => isDescending ? query.OrderByDescending(lr => lr.LeaveType.LeaveTypeName) : query.OrderBy(lr => lr.LeaveType.LeaveTypeName),
                _ => isDescending ? query.OrderByDescending(lr => lr.StartDate) : query.OrderBy(lr => lr.StartDate)
            };

            return query;
        }

        public void Update(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
        }

        public void Delete(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Remove(leaveRequest);
        }
    }
}