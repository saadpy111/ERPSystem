using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.DeleteAttendanceRecord
{
    public class DeleteAttendanceRecordHandler : IRequestHandler<DeleteAttendanceRecordRequest, DeleteAttendanceRecordResponse>
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAttendanceRecordHandler(IAttendanceRecordRepository attendanceRecordRepository, IUnitOfWork unitOfWork)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteAttendanceRecordResponse> Handle(DeleteAttendanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attendanceRecord = await _attendanceRecordRepository.GetByIdAsync(request.RecordId);
                if (attendanceRecord == null)
                {
                    return new DeleteAttendanceRecordResponse
                    {
                        Success = false,
                        Message = "Attendance record not found"
                    };
                }

                _attendanceRecordRepository.Delete(attendanceRecord);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteAttendanceRecordResponse
                {
                    Success = true,
                    Message = "Attendance record deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAttendanceRecordResponse
                {
                    Success = false,
                    Message = $"Error deleting attendance record: {ex.Message}"
                };
            }
        }
    }
}
