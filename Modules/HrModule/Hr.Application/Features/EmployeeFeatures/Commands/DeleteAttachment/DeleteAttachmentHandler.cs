using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.Commands.DeleteAttachment
{
    public class DeleteAttachmentHandler : IRequestHandler<DeleteAttachmentRequest, DeleteAttachmentResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAttachmentHandler(
            IHrAttachmentRepository attachmentRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _attachmentRepository = attachmentRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteAttachmentResponse> Handle(DeleteAttachmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId);
                if (attachment == null)
                {
                    return new DeleteAttachmentResponse
                    {
                        Success = false,
                        Message = "Attachment not found"
                    };
                }

                // Delete the file from storage
                await _fileService.DeleteFileAsync(attachment.FileUrl);

                // Delete the entity
                _attachmentRepository.Delete(attachment);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteAttachmentResponse
                {
                    Success = true,
                    Message = "Attachment deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAttachmentResponse
                {
                    Success = false,
                    Message = $"Error deleting attachment: {ex.Message}"
                };
            }
        }
    }
}