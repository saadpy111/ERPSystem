using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.MoveApplicantToStage
{
    public class MoveApplicantToStageHandler : IRequestHandler<MoveApplicantToStageRequest, MoveApplicantToStageResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IRecruitmentStageRepository _stageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveApplicantToStageHandler(
            IApplicantRepository applicantRepository,
            IRecruitmentStageRepository stageRepository,
            IUnitOfWork unitOfWork)
        {
            _applicantRepository = applicantRepository;
            _stageRepository = stageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MoveApplicantToStageResponse> Handle(MoveApplicantToStageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _applicantRepository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new MoveApplicantToStageResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                var stage = await _stageRepository.GetByIdAsync(request.NewStageId);
                if (stage == null)
                {
                    return new MoveApplicantToStageResponse
                    {
                        Success = false,
                        Message = "Recruitment stage not found"
                    };
                }

                applicant.CurrentStageId = request.NewStageId;

                applicant.Status = request.ApplicantStatus;

                if (!string.IsNullOrEmpty(request.Notes))
                {
                    applicant.InterviewNotes = request.Notes;
                }

                _applicantRepository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                return new MoveApplicantToStageResponse
                {
                    Success = true,
                    Message = $"Applicant moved to {stage.Name} stage successfully",
                    Applicant = new ApplicantDto
                    {
                        ApplicantId = applicant.ApplicantId,
                        FullName = applicant.FullName,
                        JobId = applicant.JobId,
                        JobTitle = applicant.AppliedJob?.Title ?? string.Empty,
                        ApplicationDate = applicant.ApplicationDate,
                        CurrentStageId = applicant.CurrentStageId,
                        CurrentStageName = stage.Name,
                        Status = applicant.Status,
                        ResumeUrl = applicant.ResumeUrl,
                        InterviewDate = applicant.InterviewDate
                    }
                };
            }
            catch (Exception ex)
            {
                return new MoveApplicantToStageResponse
                {
                    Success = false,
                    Message = $"Error moving applicant to stage: {ex.Message}"
                };
            }
        }
    }
}
