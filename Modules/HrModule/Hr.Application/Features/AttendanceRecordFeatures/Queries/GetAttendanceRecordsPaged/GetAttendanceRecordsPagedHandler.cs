using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.EmployeeId,
                request.StartDate,
                request.EndDate,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<AttendanceRecordDto>>(pagedResult.Items);

            return new GetAttendanceRecordsPagedResponse
            {
                PagedResult = new PagedResult<AttendanceRecordDto>
                {
                    Items = dtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}