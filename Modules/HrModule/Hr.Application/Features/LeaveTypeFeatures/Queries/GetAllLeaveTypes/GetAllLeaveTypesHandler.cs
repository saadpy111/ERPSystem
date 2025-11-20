using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetAllLeaveTypes
{
    public class GetAllLeaveTypesHandler : IRequestHandler<GetAllLeaveTypesRequest, GetAllLeaveTypesResponse>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public GetAllLeaveTypesHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        public async Task<GetAllLeaveTypesResponse> Handle(GetAllLeaveTypesRequest request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _leaveTypeRepository.GetAllAsync();
            var leaveTypeDtos = _mapper.Map<IEnumerable<LeaveTypeDto>>(leaveTypes);

            return new GetAllLeaveTypesResponse
            {
                LeaveTypes = leaveTypeDtos
            };
        }
    }
}