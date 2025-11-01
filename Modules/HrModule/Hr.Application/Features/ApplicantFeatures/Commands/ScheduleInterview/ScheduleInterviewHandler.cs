using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.ScheduleInterview
{
    public class ScheduleInterviewHandler : IRequestHandler<ScheduleInterviewRequest, ScheduleInterviewResponse>
    {
        private readonly IApplicantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleInterviewHandler(IApplicantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ScheduleInterviewResponse> Handle(ScheduleInterviewRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _repository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new ScheduleInterviewResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                applicant.InterviewDate = request.InterviewDate;
                applicant.InterviewNotes = request.InterviewNotes;
                
                // Update status if not already interviewed
                if (applicant.Status != Domain.Enums.ApplicantStatus.Interviewed &&
                    applicant.Status != Domain.Enums.ApplicantStatus.Accepted &&
                    applicant.Status != Domain.Enums.ApplicantStatus.Rejected)
                {
                    applicant.Status = Domain.Enums.ApplicantStatus.Shortlisted;
                }

                _repository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                return new ScheduleInterviewResponse
                {
                    Success = true,
                    Message = $"Interview scheduled successfully for {request.InterviewDate:yyyy-MM-dd HH:mm}",
                    Applicant = new ApplicantDto
                    {
                        ApplicantId = applicant.ApplicantId,
                        FullName = applicant.FullName,
                        JobId = applicant.JobId,
                        JobTitle = applicant.AppliedJob?.Title ?? string.Empty,
                        ApplicationDate = applicant.ApplicationDate,
                        CurrentStageId = applicant.CurrentStageId,
                        CurrentStageName = applicant.CurrentStage?.Name ?? string.Empty,
                        Status = applicant.Status,
                        ResumeUrl = applicant.ResumeUrl,
                        InterviewDate = applicant.InterviewDate
                    }
                };
            }
            catch (Exception ex)
            {
                return new ScheduleInterviewResponse
                {
                    Success = false,
                    Message = $"Error scheduling interview: {ex.Message}"
                };
            }
        }
    }
}
