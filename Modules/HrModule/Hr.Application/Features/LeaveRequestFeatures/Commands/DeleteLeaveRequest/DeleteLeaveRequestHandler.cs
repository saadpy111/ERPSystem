using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.DeleteLeaveRequest
{
    public class DeleteLeaveRequestHandler : IRequestHandler<DeleteLeaveRequestRequest, DeleteLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository, IUnitOfWork unitOfWork)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteLeaveRequestResponse> Handle(DeleteLeaveRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
                if (leaveRequest == null)
                {
                    return new DeleteLeaveRequestResponse
                    {
                        Success = false,
                        Message = "طلب الإجازة غير موجود"
                    };
                }

                _leaveRequestRepository.Delete(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteLeaveRequestResponse
                {
                    Success = true,
                    Message = "تم حذف طلب الإجازة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteLeaveRequestResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف طلب الإجازة"
                };
            }
        }
    }
}
