using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.CreateLeaveType
{
    public class CreateLeaveTypeHandler : IRequestHandler<CreateLeaveTypeRequest, CreateLeaveTypeResponse>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLeaveTypeHandler(ILeaveTypeRepository leaveTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLeaveTypeResponse> Handle(CreateLeaveTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveType = new LeaveType
                {
                    LeaveTypeName = request.LeaveTypeName,
                    DurationDays = request.DurationDays,
                    Notes = request.Notes,
                    Status = request.Status
                };

                await _leaveTypeRepository.AddAsync(leaveType);
                await _unitOfWork.SaveChangesAsync();

                var leaveTypeDto = _mapper.Map<DTOs.LeaveTypeDto>(leaveType);

                return new CreateLeaveTypeResponse
                {
                    Success = true,
                    Message = "تم إنشاء نوع الإجازة بنجاح",
                    LeaveType = leaveTypeDto
                };
            }
            catch (Exception ex)
            {
                return new CreateLeaveTypeResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء نوع الإجازة"
                };
            }
        }
    }
}