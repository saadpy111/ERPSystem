using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.AcceptApplicant
{
    public class AcceptApplicantHandler : IRequestHandler<AcceptApplicantRequest, AcceptApplicantResponse>
    {
        private readonly IApplicantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AcceptApplicantHandler(IApplicantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AcceptApplicantResponse> Handle(AcceptApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var applicant = await _repository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new AcceptApplicantResponse
                    {
                        Success = false,
                        Message = "المتقدم غير موجود"
                    };
                }

                applicant.Status = Domain.Enums.ApplicantStatus.Accepted;
                
                if (!string.IsNullOrEmpty(request.Notes))
                {
                    applicant.InterviewNotes = request.Notes;
                }

                _repository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                return new AcceptApplicantResponse
                {
                    Success = true,
                    Message = "تم قبول المتقدم بنجاح",
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
                        ResumeUrl = applicant.ResumeUrl
                    }
                };
            }
            catch (Exception ex)
            {
                return new AcceptApplicantResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء قبول المتقدم"
                };
            }
        }
    }
}
