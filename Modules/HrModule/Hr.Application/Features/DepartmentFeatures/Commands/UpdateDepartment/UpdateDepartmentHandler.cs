using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.DepartmentFeatures.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentRequest, UpdateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateDepartmentHandler(IDepartmentRepository departmentRepository, IHrAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _departmentRepository = departmentRepository;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<UpdateDepartmentResponse> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department == null)
                {
                    return new UpdateDepartmentResponse
                    {
                        Success = false,
                        Message = "Department not found"
                    };
                }

                department.Name = request.Name;
                department.Description = request.Description;
                department.ParentDepartmentId = request.ParentDepartmentId;
                department.ManagerId = request.ManagerId;

                _departmentRepository.Update(department);
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

                return new UpdateDepartmentResponse
                {
                    Success = true,
                    Message = "Department updated successfully",
                    Department = departmentDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateDepartmentResponse
                {
                    Success = false,
                    Message = $"Error updating department: {ex.Message}"
                };
            }
        }
    }
}