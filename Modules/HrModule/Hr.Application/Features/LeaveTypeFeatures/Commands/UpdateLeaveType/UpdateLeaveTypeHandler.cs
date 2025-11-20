using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeHandler : IRequestHandler<UpdateLeaveTypeRequest, UpdateLeaveTypeResponse>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLeaveTypeHandler(ILeaveTypeRepository leaveTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateLeaveTypeResponse> Handle(UpdateLeaveTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);
                if (leaveType == null)
                {
                    return new UpdateLeaveTypeResponse
                    {
                        Success = false,
                        Message = "Leave type not found"
                    };
                }

                leaveType.LeaveTypeName = request.LeaveTypeName;
                leaveType.DurationDays = request.DurationDays;
                leaveType.Notes = request.Notes;
                leaveType.Status = request.Status;

                _leaveTypeRepository.Update(leaveType);
                await _unitOfWork.SaveChangesAsync();

                var leaveTypeDto = _mapper.Map<DTOs.LeaveTypeDto>(leaveType);

                return new UpdateLeaveTypeResponse
                {
                    Success = true,
                    Message = "Leave type updated successfully",
                    LeaveType = leaveTypeDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateLeaveTypeResponse
                {
                    Success = false,
                    Message = $"Error updating leave type: {ex.Message}"
                };
            }
        }
    }
}