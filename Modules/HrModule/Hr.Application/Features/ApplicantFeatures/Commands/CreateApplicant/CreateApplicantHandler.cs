using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.CreateApplicant
{
    public class CreateApplicantHandler : IRequestHandler<CreateApplicantRequest, CreateApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateApplicantHandler(IApplicantRepository applicantRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateApplicantResponse> Handle(CreateApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = new Applicant
                {
                    FullName = request.FullName,
                    JobId = request.JobId,
                    ApplicationDate = request.ApplicationDate,
                    CurrentStageId = request.CurrentStageId,
                    Status = ApplicantStatus.Applied,
                    ResumeUrl = request.ResumeUrl,
                    QualificationsDetails = request.QualificationsDetails,
                    ExperienceDetails = request.ExperienceDetails
                };

                await _applicantRepository.AddAsync(applicant);
                await _unitOfWork.SaveChangesAsync();

                var applicantDto = _mapper.Map<DTOs.ApplicantDto>(applicant);

                return new CreateApplicantResponse
                {
                    Success = true,
                    Message = "Applicant created successfully",
                    Applicant = applicantDto
                };
            }
            catch (Exception ex)
            {
                return new CreateApplicantResponse
                {
                    Success = false,
                    Message = $"Error creating applicant: {ex.Message}"
                };
            }
        }
    }
}
