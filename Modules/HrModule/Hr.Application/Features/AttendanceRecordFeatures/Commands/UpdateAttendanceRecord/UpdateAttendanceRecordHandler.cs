using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.UpdateAttendanceRecord
{
    public class UpdateAttendanceRecordHandler : IRequestHandler<UpdateAttendanceRecordRequest, UpdateAttendanceRecordResponse>
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAttendanceRecordHandler(IAttendanceRecordRepository attendanceRecordRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateAttendanceRecordResponse> Handle(UpdateAttendanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attendanceRecord = await _attendanceRecordRepository.GetByIdAsync(request.RecordId);
                if (attendanceRecord == null)
                {
                    return new UpdateAttendanceRecordResponse
                    {
                        Success = false,
                        Message = "Attendance record not found"
                    };
                }

                attendanceRecord.EmployeeId = request.EmployeeId;
                attendanceRecord.Date = request.Date;
                attendanceRecord.CheckInTime = request.CheckInTime;
                attendanceRecord.CheckOutTime = request.CheckOutTime;
                attendanceRecord.DelayMinutes = request.DelayMinutes;

                _attendanceRecordRepository.Update(attendanceRecord);
                await _unitOfWork.SaveChangesAsync();

                var attendanceRecordDto = _mapper.Map<DTOs.AttendanceRecordDto>(attendanceRecord);

                return new UpdateAttendanceRecordResponse
                {
                    Success = true,
                    Message = "Attendance record updated successfully",
                    AttendanceRecord = attendanceRecordDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateAttendanceRecordResponse
                {
                    Success = false,
                    Message = $"Error updating attendance record: {ex.Message}"
                };
            }
        }
    }
}
