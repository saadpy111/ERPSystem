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
        private readonly IApplicantEducationRepository _applicantEducationRepository;
        private readonly IApplicantExperienceRepository _applicantExperienceRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateApplicantHandler(
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
                    // Removed ExperienceDetails assignment
                    Skills = request.Skills,
                    // Removed EducationalQualifications assignment
                    // New fields
                    Address = request.Address,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email
                };

                await _applicantRepository.AddAsync(applicant);
                await _unitOfWork.SaveChangesAsync();

                // Handle educations
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

                    await _applicantEducationRepository.AddEducations(applicant.ApplicantId, educations);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Handle experiences
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

                    await _applicantExperienceRepository.AddExperiences(applicant.ApplicantId, experiences);
                    await _unitOfWork.SaveChangesAsync();
                }

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