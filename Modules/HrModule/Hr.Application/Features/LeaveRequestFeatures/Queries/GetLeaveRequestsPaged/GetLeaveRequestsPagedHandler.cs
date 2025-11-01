using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestsPaged
{
    public class GetLeaveRequestsPagedHandler : IRequestHandler<GetLeaveRequestsPagedRequest, GetLeaveRequestsPagedResponse>
    {
        private readonly ILeaveRequestRepository _repository;
        private readonly IMapper _mapper;

        public GetLeaveRequestsPagedHandler(ILeaveRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLeaveRequestsPagedResponse> Handle(GetLeaveRequestsPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply employee filter
            if (request.EmployeeId.HasValue)
            {
                query = query.Where(lr => lr.EmployeeId == request.EmployeeId.Value);
            }

            // Apply leave type filter
            if (!string.IsNullOrWhiteSpace(request.LeaveType))
            {
                if (Enum.TryParse<Hr.Domain.Enums.LeaveType>(request.LeaveType, true, out var leaveType))
                {
                    query = query.Where(lr => lr.LeaveType == leaveType);
                }
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.LeaveRequestStatus>(request.Status, true, out var status))
                {
                    query = query.Where(lr => lr.Status == status);
                }
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(items);

            return new GetLeaveRequestsPagedResponse
            {
                PagedResult = new PagedResult<LeaveRequestDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.LeaveRequest> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.LeaveRequest> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "StartDate";

            query = orderBy.ToLower() switch
            {
                "startdate" => isDescending ? query.OrderByDescending(lr => lr.StartDate) : query.OrderBy(lr => lr.StartDate),
                "enddate" => isDescending ? query.OrderByDescending(lr => lr.EndDate) : query.OrderBy(lr => lr.EndDate),
                "durationdays" => isDescending ? query.OrderByDescending(lr => lr.DurationDays) : query.OrderBy(lr => lr.DurationDays),
                "status" => isDescending ? query.OrderByDescending(lr => lr.Status) : query.OrderBy(lr => lr.Status),
                _ => isDescending ? query.OrderByDescending(lr => lr.StartDate) : query.OrderBy(lr => lr.StartDate)
            };

            return query;
        }
    }
}
