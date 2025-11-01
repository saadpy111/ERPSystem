using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantById
{
    public class GetApplicantByIdHandler : IRequestHandler<GetApplicantByIdRequest, GetApplicantByIdResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;

        public GetApplicantByIdHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        public async Task<GetApplicantByIdResponse> Handle(GetApplicantByIdRequest request, CancellationToken cancellationToken)
        {
            var applicant = await _applicantRepository.GetByIdAsync(request.Id);
            var applicantDto = _mapper.Map<DTOs.ApplicantDto>(applicant);

            return new GetApplicantByIdResponse
            {
                Applicant = applicantDto
            };
        }
    }
}
