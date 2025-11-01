using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAllAttendanceRecords
{
    public class GetAllAttendanceRecordsHandler : IRequestHandler<GetAllAttendanceRecordsRequest, GetAllAttendanceRecordsResponse>
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IMapper _mapper;

        public GetAllAttendanceRecordsHandler(IAttendanceRecordRepository attendanceRecordRepository, IMapper mapper)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _mapper = mapper;
        }

        public async Task<GetAllAttendanceRecordsResponse> Handle(GetAllAttendanceRecordsRequest request, CancellationToken cancellationToken)
        {
            var attendanceRecords = await _attendanceRecordRepository.GetAllAsync();
            var attendanceRecordDtos = _mapper.Map<IEnumerable<AttendanceRecordDto>>(attendanceRecords);

            return new GetAllAttendanceRecordsResponse
            {
                AttendanceRecords = attendanceRecordDtos
            };
        }
    }
}
