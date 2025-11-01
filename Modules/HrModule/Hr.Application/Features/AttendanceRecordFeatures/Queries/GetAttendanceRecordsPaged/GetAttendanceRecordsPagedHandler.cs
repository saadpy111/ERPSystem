using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordsPaged
{
    public class GetAttendanceRecordsPagedHandler : IRequestHandler<GetAttendanceRecordsPagedRequest, GetAttendanceRecordsPagedResponse>
    {
        private readonly IAttendanceRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetAttendanceRecordsPagedHandler(IAttendanceRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAttendanceRecordsPagedResponse> Handle(GetAttendanceRecordsPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply employee filter
            if (request.EmployeeId.HasValue)
            {
                query = query.Where(a => a.EmployeeId == request.EmployeeId.Value);
            }

            // Apply date range filter
            if (request.StartDate.HasValue)
            {
                query = query.Where(a => a.Date >= request.StartDate.Value);
            }
            if (request.EndDate.HasValue)
            {
                query = query.Where(a => a.Date <= request.EndDate.Value);
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<AttendanceRecordDto>>(items);

            return new GetAttendanceRecordsPagedResponse
            {
                PagedResult = new PagedResult<AttendanceRecordDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.AttendanceRecord> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.AttendanceRecord> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Date";

            query = orderBy.ToLower() switch
            {
                "date" => isDescending ? query.OrderByDescending(a => a.Date) : query.OrderBy(a => a.Date),
                "checkintime" => isDescending ? query.OrderByDescending(a => a.CheckInTime) : query.OrderBy(a => a.CheckInTime),
                "checkouttime" => isDescending ? query.OrderByDescending(a => a.CheckOutTime) : query.OrderBy(a => a.CheckOutTime),
                "delayminutes" => isDescending ? query.OrderByDescending(a => a.DelayMinutes) : query.OrderBy(a => a.DelayMinutes),
                _ => isDescending ? query.OrderByDescending(a => a.Date) : query.OrderBy(a => a.Date)
            };

            return query;
        }
    }
}
