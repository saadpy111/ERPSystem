using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantById
{
    public class GetApplicantByIdHandler : IRequestHandler<GetApplicantByIdRequest, GetApplicantByIdResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetApplicantByIdHandler(IApplicantRepository applicantRepository, IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetApplicantByIdResponse> Handle(GetApplicantByIdRequest request, CancellationToken cancellationToken)
        {
            var applicant = await _applicantRepository.GetByIdAsync(request.Id);
            var applicantDto = _mapper.Map<DTOs.ApplicantDto>(applicant);

            // Fetch attachments for this applicant
            if (applicantDto != null)
            {
                var attachments = await _attachmentRepository.GetByEntityAsync("Applicant", applicantDto.ApplicantId);
                applicantDto.Attachments = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);
            }

            return new GetApplicantByIdResponse
            {
                Applicant = applicantDto
            };
        }
    }
}
