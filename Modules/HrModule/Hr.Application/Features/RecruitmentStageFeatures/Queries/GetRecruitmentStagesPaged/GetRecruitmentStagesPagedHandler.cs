using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStagesPaged
{
    public class GetRecruitmentStagesPagedHandler : IRequestHandler<GetRecruitmentStagesPagedRequest, GetRecruitmentStagesPagedResponse>
    {
        private readonly IRecruitmentStageRepository _repository;
        private readonly IMapper _mapper;

        public GetRecruitmentStagesPagedHandler(IRecruitmentStageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetRecruitmentStagesPagedResponse> Handle(GetRecruitmentStagesPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(rs => rs.Name.ToLower().Contains(searchTerm));
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = items.Select(rs => new
            {
                rs.StageId,
                rs.Name,
                rs.SequenceOrder
            });

            return new GetRecruitmentStagesPagedResponse
            {
                PagedResult = new PagedResult<object>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.RecruitmentStage> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.RecruitmentStage> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "SequenceOrder";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(rs => rs.Name) : query.OrderBy(rs => rs.Name),
                "sequenceorder" => isDescending ? query.OrderByDescending(rs => rs.SequenceOrder) : query.OrderBy(rs => rs.SequenceOrder),
                _ => isDescending ? query.OrderByDescending(rs => rs.SequenceOrder) : query.OrderBy(rs => rs.SequenceOrder)
            };

            return query;
        }
    }
}
