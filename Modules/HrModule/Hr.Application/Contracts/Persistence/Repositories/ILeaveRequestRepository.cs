using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
        void Update(LeaveRequest leaveRequest);
        void Delete(LeaveRequest leaveRequest);
    }
}
