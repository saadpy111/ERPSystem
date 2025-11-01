using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.UpdateLeaveRequest
{
    public class UpdateLeaveRequestHandler : IRequestHandler<UpdateLeaveRequestRequest, UpdateLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateLeaveRequestResponse> Handle(UpdateLeaveRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
                if (leaveRequest == null)
                {
                    return new UpdateLeaveRequestResponse
                    {
                        Success = false,
                        Message = "Leave request not found"
                    };
                }

                leaveRequest.EmployeeId = request.EmployeeId;
                leaveRequest.LeaveType = request.LeaveType;
                leaveRequest.StartDate = request.StartDate;
                leaveRequest.EndDate = request.EndDate;
                leaveRequest.DurationDays = request.DurationDays;
                leaveRequest.Status = request.Status;

                _leaveRequestRepository.Update(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                var leaveRequestDto = _mapper.Map<DTOs.LeaveRequestDto>(leaveRequest);

                return new UpdateLeaveRequestResponse
                {
                    Success = true,
                    Message = "Leave request updated successfully",
                    LeaveRequest = leaveRequestDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateLeaveRequestResponse
                {
                    Success = false,
                    Message = $"Error updating leave request: {ex.Message}"
                };
            }
        }
    }
}
