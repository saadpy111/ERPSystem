using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.CreateAttendanceRecord
{
    public class CreateAttendanceRecordHandler : IRequestHandler<CreateAttendanceRecordRequest, CreateAttendanceRecordResponse>
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAttendanceRecordHandler(IAttendanceRecordRepository attendanceRecordRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateAttendanceRecordResponse> Handle(CreateAttendanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attendanceRecord = new AttendanceRecord
                {
                    EmployeeId = request.EmployeeId,
                    Date = request.Date,
                    CheckInTime = request.CheckInTime,
                    CheckOutTime = request.CheckOutTime,
                    DelayMinutes = request.DelayMinutes
                };

                await _attendanceRecordRepository.AddAsync(attendanceRecord);
                await _unitOfWork.SaveChangesAsync();

                var attendanceRecordDto = _mapper.Map<DTOs.AttendanceRecordDto>(attendanceRecord);

                return new CreateAttendanceRecordResponse
                {
                    Success = true,
                    Message = "تم إنشاء سجل الحضور بنجاح",
                    AttendanceRecord = attendanceRecordDto
                };
            }
            catch (Exception ex)
            {
                return new CreateAttendanceRecordResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء سجل الحضور"
                };
            }
        }
    }
}
