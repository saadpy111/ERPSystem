using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsPaged
{
    public class GetApplicantsPagedHandler : IRequestHandler<GetApplicantsPagedRequest, GetApplicantsPagedResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;

        public GetApplicantsPagedHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        public async Task<GetApplicantsPagedResponse> Handle(GetApplicantsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _applicantRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.JobId,
                request.CurrentStageId,
                request.Status,
                request.OrderBy,
                request.IsDescending);

            var applicantDtos = _mapper.Map<IEnumerable<ApplicantDto>>(pagedResult.Items);

            return new GetApplicantsPagedResponse
            {
                PagedResult = new PagedResult<ApplicantDto>
                {
                    Items = applicantDtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}