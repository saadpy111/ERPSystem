using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestById
{
    public class GetLeaveRequestByIdHandler : IRequestHandler<GetLeaveRequestByIdRequest, GetLeaveRequestByIdResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestByIdHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<GetLeaveRequestByIdResponse> Handle(GetLeaveRequestByIdRequest request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            var leaveRequestDto = _mapper.Map<DTOs.LeaveRequestDto>(leaveRequest);

            return new GetLeaveRequestByIdResponse
            {
                LeaveRequest = leaveRequestDto
            };
        }
    }
}
