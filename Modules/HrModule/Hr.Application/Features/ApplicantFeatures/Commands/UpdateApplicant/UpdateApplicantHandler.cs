using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.UpdateApplicant
{
    public class UpdateApplicantHandler : IRequestHandler<UpdateApplicantRequest, UpdateApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateApplicantHandler(IApplicantRepository applicantRepository, IHrAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _applicantRepository = applicantRepository;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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

                // Handle attachment files
                if (request.AttachmentFiles != null && request.AttachmentFiles.Any())
                {
                    foreach (var file in request.AttachmentFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileUrl = await _fileService.SaveFileAsync(file, "applicants");
                            var attachment = new HrAttachment
                            {
                                FileName = file.FileName,
                                FileUrl = fileUrl,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                EntityType = "Applicant",
                                EntityId = applicant.ApplicantId,
                                Description = $"Attachment for Applicant {applicant.FullName}",
                                UploadedAt = DateTime.UtcNow
                            };
                            await _attachmentRepository.AddAsync(attachment);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

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
