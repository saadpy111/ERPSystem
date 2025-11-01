using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetAllApplicants
{
    public class GetAllApplicantsHandler : IRequestHandler<GetAllApplicantsRequest, GetAllApplicantsResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;

        public GetAllApplicantsHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        public async Task<GetAllApplicantsResponse> Handle(GetAllApplicantsRequest request, CancellationToken cancellationToken)
        {
            var applicants = await _applicantRepository.GetAllAsync();
            var applicantDtos = _mapper.Map<IEnumerable<ApplicantDto>>(applicants);

            return new GetAllApplicantsResponse
            {
                Applicants = applicantDtos
            };
        }
    }
}
