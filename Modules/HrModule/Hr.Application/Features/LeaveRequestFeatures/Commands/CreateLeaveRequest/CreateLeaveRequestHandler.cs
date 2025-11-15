using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest
{
    public class CreateLeaveRequestHandler : IRequestHandler<CreateLeaveRequestRequest, CreateLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLeaveRequestResponse> Handle(CreateLeaveRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = request.EmployeeId,
                    LeaveType = request.LeaveType,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DurationDays = request.DurationDays,
                    Status = LeaveRequestStatus.Pending,
                    Notes = request.Notes
                };

                await _leaveRequestRepository.AddAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                var leaveRequestDto = _mapper.Map<DTOs.LeaveRequestDto>(leaveRequest);

                return new CreateLeaveRequestResponse
                {
                    Success = true,
                    Message = "Leave request created successfully",
                    LeaveRequest = leaveRequestDto
                };
            }
            catch (Exception ex)
            {
                return new CreateLeaveRequestResponse
                {
                    Success = false,
                    Message = $"Error creating leave request: {ex.Message}"
                };
            }
        }
    }
}