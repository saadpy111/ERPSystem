using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.GetAllLeaveRequests
{
    public class GetAllLeaveRequestsHandler : IRequestHandler<GetAllLeaveRequestsRequest, GetAllLeaveRequestsResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetAllLeaveRequestsHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<GetAllLeaveRequestsResponse> Handle(GetAllLeaveRequestsRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestRepository.GetAllAsync();
            var leaveRequestDtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(leaveRequests);

            return new GetAllLeaveRequestsResponse
            {
                LeaveRequests = leaveRequestDtos
            };
        }
    }
}
