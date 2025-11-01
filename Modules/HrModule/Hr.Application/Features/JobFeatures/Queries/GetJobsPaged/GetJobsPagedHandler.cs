using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

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
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(j => j.Title.ToLower().Contains(searchTerm));
            }

            // Apply department filter
            if (request.DepartmentId.HasValue)
            {
                query = query.Where(j => j.DepartmentId == request.DepartmentId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.JobStatus>(request.Status, true, out var status))
                {
                    query = query.Where(j => j.Status == status);
                }
            }

            // Apply work type filter
            if (!string.IsNullOrWhiteSpace(request.WorkType))
            {
                if (Enum.TryParse<Hr.Domain.Enums.WorkType>(request.WorkType, true, out var workType))
                {
                    query = query.Where(j => j.WorkType == workType);
                }
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<JobDto>>(items);

            return new GetJobsPagedResponse
            {
                PagedResult = new PagedResult<JobDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.Job> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.Job> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Title";

            query = orderBy.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title),
                "publisheddate" => isDescending ? query.OrderByDescending(j => j.PublishedDate) : query.OrderBy(j => j.PublishedDate),
                "status" => isDescending ? query.OrderByDescending(j => j.Status) : query.OrderBy(j => j.Status),
                _ => isDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title)
            };

            return query;
        }
    }
}
