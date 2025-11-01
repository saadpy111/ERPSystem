using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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
                .FirstOrDefaultAsync(lr => lr.RequestId == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.EmployeeId == employeeId)
                .Include(lr => lr.Employee)
                .ToListAsync();
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
