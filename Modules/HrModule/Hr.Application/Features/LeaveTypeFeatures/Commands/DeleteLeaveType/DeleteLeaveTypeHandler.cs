using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeHandler : IRequestHandler<DeleteLeaveTypeRequest, DeleteLeaveTypeResponse>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveTypeHandler(ILeaveTypeRepository leaveTypeRepository, IUnitOfWork unitOfWork)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteLeaveTypeResponse> Handle(DeleteLeaveTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);
                if (leaveType == null)
                {
                    return new DeleteLeaveTypeResponse
                    {
                        Success = false,
                        Message = "Leave type not found"
                    };
                }

                _leaveTypeRepository.Delete(leaveType);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteLeaveTypeResponse
                {
                    Success = true,
                    Message = "Leave type deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteLeaveTypeResponse
                {
                    Success = false,
                    Message = $"Error deleting leave type: {ex.Message}"
                };
            }
        }
    }
}