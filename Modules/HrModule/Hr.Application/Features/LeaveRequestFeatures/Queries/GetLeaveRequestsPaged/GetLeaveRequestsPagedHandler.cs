using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestsPaged
{
    public class GetLeaveRequestsPagedHandler : IRequestHandler<GetLeaveRequestsPagedRequest, GetLeaveRequestsPagedResponse>
    {
        private readonly ILeaveRequestRepository _repository;
        private readonly IMapper _mapper;

        public GetLeaveRequestsPagedHandler(ILeaveRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLeaveRequestsPagedResponse> Handle(GetLeaveRequestsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.EmployeeId,
                request.LeaveType,
                request.Status,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(pagedResult.Items);

            return new GetLeaveRequestsPagedResponse
            {
                PagedResult = new PagedResult<LeaveRequestDto>
                {
                    Items = dtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}