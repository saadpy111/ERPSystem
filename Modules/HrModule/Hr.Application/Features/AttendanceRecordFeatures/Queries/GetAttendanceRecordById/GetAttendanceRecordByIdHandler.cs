using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordById
{
    public class GetAttendanceRecordByIdHandler : IRequestHandler<GetAttendanceRecordByIdRequest, GetAttendanceRecordByIdResponse>
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IMapper _mapper;

        public GetAttendanceRecordByIdHandler(IAttendanceRecordRepository attendanceRecordRepository, IMapper mapper)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _mapper = mapper;
        }

        public async Task<GetAttendanceRecordByIdResponse> Handle(GetAttendanceRecordByIdRequest request, CancellationToken cancellationToken)
        {
            var attendanceRecord = await _attendanceRecordRepository.GetByIdAsync(request.Id);
            var attendanceRecordDto = _mapper.Map<DTOs.AttendanceRecordDto>(attendanceRecord);

            return new GetAttendanceRecordByIdResponse
            {
                AttendanceRecord = attendanceRecordDto
            };
        }
    }
}
