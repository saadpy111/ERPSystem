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
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly HrDbContext _context;

        public LeaveTypeRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveType> AddAsync(LeaveType leaveType)
        {
            await _context.LeaveTypes.AddAsync(leaveType);
            return leaveType;
        }

        public async Task<LeaveType?> GetByIdAsync(int id)
        {
            return await _context.LeaveTypes.FindAsync(id);
        }

        public async Task<IEnumerable<LeaveType>> GetAllAsync()
        {
            return await _context.LeaveTypes.ToListAsync();
        }

        public async Task<PagedResult<LeaveType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false, string? status = null)
        {
            var query = _context.LeaveTypes.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(lt => lt.LeaveTypeName.ToLower().Contains(searchTermLower));
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<LeaveTypeStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(lt => lt.Status == statusEnum);
                }
            }

            var totalCount = await query.CountAsync();

            // Apply ordering
            query = ApplyOrdering(query, orderBy, isDescending);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LeaveType>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        private IQueryable<LeaveType> ApplyOrdering(IQueryable<LeaveType> query, string? orderBy, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "LeaveTypeName";

            query = orderBy.ToLower() switch
            {
                "leavetypename" => isDescending ? query.OrderByDescending(lt => lt.LeaveTypeName) : query.OrderBy(lt => lt.LeaveTypeName),
                "durationdays" => isDescending ? query.OrderByDescending(lt => lt.DurationDays) : query.OrderBy(lt => lt.DurationDays),
                "status" => isDescending ? query.OrderByDescending(lt => lt.Status) : query.OrderBy(lt => lt.Status),
                _ => isDescending ? query.OrderByDescending(lt => lt.LeaveTypeName) : query.OrderBy(lt => lt.LeaveTypeName)
            };

            return query;
        }

        public void Update(LeaveType leaveType)
        {
            _context.LeaveTypes.Update(leaveType);
        }

        public void Delete(LeaveType leaveType)
        {
            _context.LeaveTypes.Remove(leaveType);
        }
    }
}