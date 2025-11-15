using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest> AddAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
        Task<PagedResult<LeaveRequest>> GetPagedAsync(int pageNumber, int pageSize, int? employeeId = null, string? leaveType = null, string? status = null, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        void Update(LeaveRequest leaveRequest);
        void Delete(LeaveRequest leaveRequest);
    }
}