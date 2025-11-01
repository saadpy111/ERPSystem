using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.UpdateApplicant
{
    public class UpdateApplicantHandler : IRequestHandler<UpdateApplicantRequest, UpdateApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateApplicantHandler(IApplicantRepository applicantRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateApplicantResponse> Handle(UpdateApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _applicantRepository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new UpdateApplicantResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                applicant.FullName = request.FullName;
                applicant.JobId = request.JobId;
                applicant.ApplicationDate = request.ApplicationDate;
                applicant.CurrentStageId = request.CurrentStageId;
                applicant.Status = request.Status;
                applicant.ResumeUrl = request.ResumeUrl;
                applicant.QualificationsDetails = request.QualificationsDetails;
                applicant.ExperienceDetails = request.ExperienceDetails;

                _applicantRepository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                var applicantDto = _mapper.Map<DTOs.ApplicantDto>(applicant);

                return new UpdateApplicantResponse
                {
                    Success = true,
                    Message = "Applicant updated successfully",
                    Applicant = applicantDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateApplicantResponse
                {
                    Success = false,
                    Message = $"Error updating applicant: {ex.Message}"
                };
            }
        }
    }
}
