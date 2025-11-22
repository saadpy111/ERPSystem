using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.UpdateApplicant
{
    public class UpdateApplicantHandler : IRequestHandler<UpdateApplicantRequest, UpdateApplicantResponse>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantEducationRepository _applicantEducationRepository;
        private readonly IApplicantExperienceRepository _applicantExperienceRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateApplicantHandler(
            IApplicantRepository applicantRepository,
            IApplicantEducationRepository applicantEducationRepository,
            IApplicantExperienceRepository applicantExperienceRepository,
            IHrAttachmentRepository attachmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _applicantRepository = applicantRepository;
            _applicantEducationRepository = applicantEducationRepository;
            _applicantExperienceRepository = applicantExperienceRepository;
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
                // Removed ExperienceDetails assignment
                applicant.Skills = request.Skills;
                // Removed EducationalQualifications assignment
                // New fields
                applicant.Address = request.Address;
                applicant.Gender = request.Gender;
                applicant.PhoneNumber = request.PhoneNumber;
                applicant.DateOfBirth = request.DateOfBirth;
                applicant.Email = request.Email;

                _applicantRepository.Update(applicant);
                await _unitOfWork.SaveChangesAsync();

                // Handle educations - first remove existing educations, then add new ones
                await _applicantEducationRepository.RemoveEducations(request.ApplicantId);
                if (request.Educations != null && request.Educations.Any())
                {
                    var educations = request.Educations.Select(dto => new ApplicantEducation
                    {
                        DegreeName = dto.DegreeName,
                        Specialization = dto.Specialization,
                        GraduationYear = dto.GraduationYear,
                        Institute = dto.Institute,
                        Grade = dto.Grade
                    }).ToList();

                    await _applicantEducationRepository.AddEducations(request.ApplicantId, educations);
                }
                await _unitOfWork.SaveChangesAsync();

                // Handle experiences - first remove existing experiences, then add new ones
                await _applicantExperienceRepository.RemoveExperiences(request.ApplicantId);
                if (request.Experiences != null && request.Experiences.Any())
                {
                    var experiences = request.Experiences.Select(dto => new ApplicantExperience
                    {
                        CompanyName = dto.CompanyName,
                        JobTitle = dto.JobTitle,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        Description = dto.Description
                    }).ToList();

                    await _applicantExperienceRepository.AddExperiences(request.ApplicantId, experiences);
                }
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