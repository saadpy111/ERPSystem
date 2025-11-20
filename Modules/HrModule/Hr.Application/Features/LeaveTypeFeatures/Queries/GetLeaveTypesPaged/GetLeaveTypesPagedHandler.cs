using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypesPaged
{
    public class GetLeaveTypesPagedHandler : IRequestHandler<GetLeaveTypesPagedRequest, GetLeaveTypesPagedResponse>
    {
        private readonly ILeaveTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetLeaveTypesPagedHandler(ILeaveTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLeaveTypesPagedResponse> Handle(GetLeaveTypesPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending,
                request.Status);

            var dtos = _mapper.Map<IEnumerable<LeaveTypeDto>>(pagedResult.Items);

            return new GetLeaveTypesPagedResponse
            {
                PagedResult = new PagedResult<LeaveTypeDto>
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