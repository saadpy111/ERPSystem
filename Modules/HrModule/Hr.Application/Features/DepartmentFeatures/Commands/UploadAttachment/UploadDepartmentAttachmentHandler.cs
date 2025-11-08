using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using System;

namespace Hr.Application.Features.DepartmentFeatures.Commands.UploadAttachment
{
    public class UploadDepartmentAttachmentHandler : IRequestHandler<UploadDepartmentAttachmentRequest, UploadDepartmentAttachmentResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UploadDepartmentAttachmentHandler(
            IHrAttachmentRepository attachmentRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UploadDepartmentAttachmentResponse> Handle(UploadDepartmentAttachmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.AttachmentFile == null)
                {
                    return new UploadDepartmentAttachmentResponse
                    {
                        Success = false,
                        Message = "No attachment file provided"
                    };
                }

                // Save the file using the file service
                var folderPath = $"departments/{request.DepartmentId}/attachments";
                var fileUrl = await _fileService.SaveFileAsync(request.AttachmentFile, folderPath);

                // Create HrAttachment entity
                var attachment = new HrAttachment
                {
                    FileName = request.AttachmentFile.FileName,
                    FileUrl = fileUrl,
                    ContentType = request.AttachmentFile.ContentType,
                    FileSize = request.AttachmentFile.Length,
                    EntityType = "Department",
                    EntityId = request.DepartmentId,
                    Description = request.Description,
                    UploadedAt = DateTime.UtcNow
                };

                await _attachmentRepository.AddAsync(attachment);
                await _unitOfWork.SaveChangesAsync();

                var attachmentDto = _mapper.Map<DTOs.HrAttachmentDto>(attachment);

                return new UploadDepartmentAttachmentResponse
                {
                    Success = true,
                    Message = "Attachment uploaded successfully",
                    Attachment = attachmentDto
                };
            }
            catch (Exception ex)
            {
                return new UploadDepartmentAttachmentResponse
                {
                    Success = false,
                    Message = $"Error uploading attachment: {ex.Message}"
                };
            }
        }
    }
}