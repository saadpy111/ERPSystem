using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending);

            var dtos = pagedResult.Items.Select(rs => new
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
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}