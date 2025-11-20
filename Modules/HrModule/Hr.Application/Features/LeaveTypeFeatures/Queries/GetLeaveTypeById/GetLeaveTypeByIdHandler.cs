using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypeById
{
    public class GetLeaveTypeByIdHandler : IRequestHandler<GetLeaveTypeByIdRequest, GetLeaveTypeByIdResponse>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public GetLeaveTypeByIdHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task<GetLeaveTypeByIdResponse> Handle(GetLeaveTypeByIdRequest request, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);
            var leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveType);

            return new GetLeaveTypeByIdResponse
            {
                LeaveType = leaveTypeDto
            };
        }
    }
}