using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface ILeaveTypeRepository
    {
        Task<LeaveType> AddAsync(LeaveType leaveType);
        Task<LeaveType?> GetByIdAsync(int id);
        Task<IEnumerable<LeaveType>> GetAllAsync();
        Task<PagedResult<LeaveType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false, string? status = null);
        void Update(LeaveType leaveType);
        void Delete(LeaveType leaveType);
    }
}