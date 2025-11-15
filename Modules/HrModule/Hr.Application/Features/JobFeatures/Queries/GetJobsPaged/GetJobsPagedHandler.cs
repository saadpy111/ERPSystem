using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.JobFeatures.GetJobsPaged
{
    public class GetJobsPagedHandler : IRequestHandler<GetJobsPagedRequest, GetJobsPagedResponse>
    {
        private readonly IJobRepository _repository;
        private readonly IMapper _mapper;

        public GetJobsPagedHandler(IJobRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetJobsPagedResponse> Handle(GetJobsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.DepartmentId,
                request.Status,
                request.WorkType,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<JobDto>>(pagedResult.Items);

            return new GetJobsPagedResponse
            {
                PagedResult = new PagedResult<JobDto>
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