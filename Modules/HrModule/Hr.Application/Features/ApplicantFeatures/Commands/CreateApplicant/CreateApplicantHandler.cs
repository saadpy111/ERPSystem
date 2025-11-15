using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.CreateApplicant
{
    public class CreateApplicantHandler : IRequestHandler<CreateApplicantRequest, CreateApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateApplicantHandler(IApplicantRepository applicantRepository, IHrAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _applicantRepository = applicantRepository;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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
                    ExperienceDetails = request.ExperienceDetails,
                    Skills = request.Skills,
                    EducationalQualifications = request.EducationalQualifications
                };

                await _applicantRepository.AddAsync(applicant);
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