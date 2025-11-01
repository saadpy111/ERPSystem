using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

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
            var query = (await _applicantRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(a => a.FullName.ToLower().Contains(searchTerm));
            }

            // Apply job filter
            if (request.JobId.HasValue)
            {
                query = query.Where(a => a.JobId == request.JobId.Value);
            }

            // Apply stage filter
            if (request.CurrentStageId.HasValue)
            {
                query = query.Where(a => a.CurrentStageId == request.CurrentStageId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.ApplicantStatus>(request.Status, true, out var status))
                {
                    query = query.Where(a => a.Status == status);
                }
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var applicants = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var applicantDtos = _mapper.Map<IEnumerable<ApplicantDto>>(applicants);

            return new GetApplicantsPagedResponse
            {
                PagedResult = new PagedResult<ApplicantDto>
                {
                    Items = applicantDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.Applicant> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.Applicant> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "FullName";

            query = orderBy.ToLower() switch
            {
                "fullname" => isDescending ? query.OrderByDescending(a => a.FullName) : query.OrderBy(a => a.FullName),
                "applicationdate" => isDescending ? query.OrderByDescending(a => a.ApplicationDate) : query.OrderBy(a => a.ApplicationDate),
                "status" => isDescending ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status),
                _ => isDescending ? query.OrderByDescending(a => a.FullName) : query.OrderBy(a => a.FullName)
            };

            return query;
        }
    }
}
