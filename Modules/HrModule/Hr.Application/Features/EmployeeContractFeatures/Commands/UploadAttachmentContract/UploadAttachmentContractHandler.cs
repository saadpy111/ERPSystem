using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.UploadAttachmentContract
{
    public class UploadAttachmentContractHandler : IRequestHandler<UploadAttachmentContractRequest, UploadAttachmentContractResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UploadAttachmentContractHandler(
            IHrAttachmentRepository attachmentRepository,
            IEmployeeContractRepository employeeContractRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _attachmentRepository = attachmentRepository;
            _employeeContractRepository = employeeContractRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<UploadAttachmentContractResponse> Handle(UploadAttachmentContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if employee contract exists
                var employeeContract = await _employeeContractRepository.GetByIdAsync(request.EmployeeContractId);
                if (employeeContract == null)
                {
                    return new UploadAttachmentContractResponse
                    {
                        Success = false,
                        Message = "Employee contract not found"
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
                            var fileUrl = await _fileService.SaveFileAsync(file, "employee-contracts");
                            var attachment = new HrAttachment
                            {
                                FileName = file.FileName,
                                FileUrl = fileUrl,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                EntityType = "EmployeeContract",
                                EntityId = employeeContract.Id,
                                Description = $"Attachment for Employee Contract {employeeContract.Id}",
                                UploadedAt = DateTime.UtcNow
                            };
                            await _attachmentRepository.AddAsync(attachment);
                            attachments.Add(attachment);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                var attachmentDtos = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);

                return new UploadAttachmentContractResponse
                {
                    Success = true,
                    Message = "Attachments uploaded successfully",
                    Attachments = attachmentDtos
                };
            }
            catch (Exception ex)
            {
                return new UploadAttachmentContractResponse
                {
                    Success = false,
                    Message = $"Error uploading attachments: {ex.Message}"
                };
            }
        }
    }
}