using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.DepartmentFeatures.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentRequest, CreateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IHrAttachmentRepository attachmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _departmentRepository = departmentRepository;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<CreateDepartmentResponse> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var department = new Department
                {
                    Name = request.Name,
                    Description = request.Description,
                    ParentDepartmentId = request.ParentDepartmentId,
                    ManagerId = request.ManagerId,
                    CreatedAt = DateTime.UtcNow
                };

                await _departmentRepository.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                // Handle attachment files
                if (request.AttachmentFiles != null && request.AttachmentFiles.Any())
                {
                    foreach (var file in request.AttachmentFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileUrl = await _fileService.SaveFileAsync(file, "departments");
                            var attachment = new HrAttachment
                            {
                                FileName = file.FileName,
                                FileUrl = fileUrl,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                EntityType = "Department",
                                EntityId = department.DepartmentId,
                                Description = $"Attachment for Department {department.Name}",
                                UploadedAt = DateTime.UtcNow
                            };
                            await _attachmentRepository.AddAsync(attachment);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                var departmentDto = _mapper.Map<DTOs.DepartmentDto>(department);

                return new CreateDepartmentResponse
                {
                    Success = true,
                    Message = "تم إنشاء القسم بنجاح",
                    Department = departmentDto
                };
            }
            catch (Exception ex)
            {
                return new CreateDepartmentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء القسم"
                };
            }
        }
    }
}