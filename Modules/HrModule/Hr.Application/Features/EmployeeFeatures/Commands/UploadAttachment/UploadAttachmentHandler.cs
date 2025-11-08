using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using System;

namespace Hr.Application.Features.EmployeeFeatures.Commands.UploadAttachment
{
    public class UploadAttachmentHandler : IRequestHandler<UploadAttachmentRequest, UploadAttachmentResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UploadAttachmentHandler(
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

        public async Task<UploadAttachmentResponse> Handle(UploadAttachmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.AttachmentFile == null)
                {
                    return new UploadAttachmentResponse
                    {
                        Success = false,
                        Message = "No attachment file provided"
                    };
                }

                // Save the file using the file service
                var folderPath = $"employees/{request.EmployeeId}/attachments";
                var fileUrl = await _fileService.SaveFileAsync(request.AttachmentFile, folderPath);

                // Create HrAttachment entity
                var attachment = new HrAttachment
                {
                    FileName = request.AttachmentFile.FileName,
                    FileUrl = fileUrl,
                    ContentType = request.AttachmentFile.ContentType,
                    FileSize = request.AttachmentFile.Length,
                    EntityType = "Employee",
                    EntityId = request.EmployeeId,
                    Description = request.Description,
                    UploadedAt = DateTime.UtcNow
                };

                await _attachmentRepository.AddAsync(attachment);
                await _unitOfWork.SaveChangesAsync();

                var attachmentDto = _mapper.Map<DTOs.HrAttachmentDto>(attachment);

                return new UploadAttachmentResponse
                {
                    Success = true,
                    Message = "Attachment uploaded successfully",
                    Attachment = attachmentDto
                };
            }
            catch (Exception ex)
            {
                return new UploadAttachmentResponse
                {
                    Success = false,
                    Message = $"Error uploading attachment: {ex.Message}"
                };
            }
        }
    }
}