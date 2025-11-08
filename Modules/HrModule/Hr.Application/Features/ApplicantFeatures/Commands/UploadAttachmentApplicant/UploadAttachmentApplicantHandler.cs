using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.Commands.UploadAttachmentApplicant
{
    public class UploadAttachmentApplicantHandler : IRequestHandler<UploadAttachmentApplicantRequest, UploadAttachmentApplicantResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UploadAttachmentApplicantHandler(
            IHrAttachmentRepository attachmentRepository,
            IApplicantRepository applicantRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _attachmentRepository = attachmentRepository;
            _applicantRepository = applicantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<UploadAttachmentApplicantResponse> Handle(UploadAttachmentApplicantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if applicant exists
                var applicant = await _applicantRepository.GetByIdAsync(request.ApplicantId);
                if (applicant == null)
                {
                    return new UploadAttachmentApplicantResponse
                    {
                        Success = false,
                        Message = "Applicant not found"
                    };
                }

                var attachments = new List<HrAttachment>();

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
                            attachments.Add(attachment);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                var attachmentDtos = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);

                return new UploadAttachmentApplicantResponse
                {
                    Success = true,
                    Message = "Attachments uploaded successfully",
                    Attachments = attachmentDtos
                };
            }
            catch (Exception ex)
            {
                return new UploadAttachmentApplicantResponse
                {
                    Success = false,
                    Message = $"Error uploading attachments: {ex.Message}"
                };
            }
        }
    }
}