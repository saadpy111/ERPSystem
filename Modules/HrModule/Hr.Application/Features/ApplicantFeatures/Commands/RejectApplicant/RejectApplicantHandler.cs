using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.RejectApplicant
{
    public class RejectApplicantHandler : IRequestHandler<RejectApplicantRequest, RejectApplicantResponse>
    {
        private readonly IApplicantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectApplicantHandler(IApplicantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RejectApplicantResponse> Handle(RejectApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _repository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new RejectApplicantResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                applicant.Status = Domain.Enums.ApplicantStatus.Rejected;
                
                if (!string.IsNullOrEmpty(request.RejectionReason))
                {
                    applicant.InterviewNotes = $"Rejection Reason: {request.RejectionReason}";
                }

                _repository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                return new RejectApplicantResponse
                {
                    Success = true,
                    Message = "Applicant rejected successfully",
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
                    }
                };
            }
            catch (Exception ex)
            {
                return new RejectApplicantResponse
                {
                    Success = false,
                    Message = $"Error rejecting applicant: {ex.Message}"
                };
            }
        }
    }
}
