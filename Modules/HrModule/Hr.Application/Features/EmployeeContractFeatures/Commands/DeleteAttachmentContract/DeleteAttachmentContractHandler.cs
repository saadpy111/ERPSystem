using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteAttachmentContract
{
    public class DeleteAttachmentContractHandler : IRequestHandler<DeleteAttachmentContractRequest, DeleteAttachmentContractResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAttachmentContractHandler(
            IHrAttachmentRepository attachmentRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _attachmentRepository = attachmentRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteAttachmentContractResponse> Handle(DeleteAttachmentContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId);
                if (attachment == null)
                {
                    return new DeleteAttachmentContractResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على المرفق"
                    };
                }

                // Delete the file from storage
                await _fileService.DeleteFileAsync(attachment.FileUrl);

                // Delete the entity
                _attachmentRepository.Delete(attachment);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteAttachmentContractResponse
                {
                    Success = true,
                    Message = "تم حذف المرفق بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAttachmentContractResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف المرفق"
                };
            }
        }
    }
}