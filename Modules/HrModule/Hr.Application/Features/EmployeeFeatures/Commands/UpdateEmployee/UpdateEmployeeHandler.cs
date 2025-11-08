using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using System;
using System.Linq;

namespace Hr.Application.Features.EmployeeFeatures.UpdateEmployee
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeRequest, UpdateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileService _fileService;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEmployeeHandler(IEmployeeRepository employeeRepository, IFileService fileService, IHrAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _fileService = fileService;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateEmployeeResponse> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.Id);
                if (employee == null)
                {
                    return new UpdateEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }
                #region BaseFileds
                employee.FullName = request.FullName;
                employee.Email = request.Email;
                employee.PhoneNumber = request.PhoneNumber;
                employee.Status = request.Status;
                
              
                if (Enum.TryParse<Hr.Domain.Enums.Gender>(request.Gender, out var gender))
                {
                    employee.Gender = gender;
                }
                employee.Address = request.Address;
                employee.ManagerId = request.ManagerId;
                #endregion



                #region Handle image file if provided
                if (request.ImageFile != null)
                {

                    if (employee.ImageUrl != null)
                    {
                        await _fileService.DeleteFileAsync(employee.ImageUrl);
                    }
                    var folderPath = $"employeesImages";
                    var imageUrl = await _fileService.SaveFileAsync(request.ImageFile, folderPath);
                    employee.ImageUrl = imageUrl;
                }
                #endregion


                #region Handle attachment files if provided
                if (request.AttachmentFiles != null && request.AttachmentFiles.Any())
                {
                    foreach (var attachmentFile in request.AttachmentFiles)
                    {
                        var folderPath = $"employees/{employee.EmployeeId}/attachments";
                        var fileUrl = await _fileService.SaveFileAsync(attachmentFile, folderPath);
                        
                        var attachment = new HrAttachment
                        {
                            FileName = attachmentFile.FileName,
                            FileUrl = fileUrl,
                            ContentType = attachmentFile.ContentType,
                            FileSize = attachmentFile.Length,
                            EntityType = "Employee",
                            EntityId = employee.EmployeeId,
                            UploadedAt = DateTime.UtcNow
                        };
                        
                        await _attachmentRepository.AddAsync(attachment);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                #endregion

                _employeeRepository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

                return new UpdateEmployeeResponse
                {
                    Success = true,
                    Message = "Employee updated successfully",
                    Employee = employeeDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error updating employee: {ex.Message}"
                };
            }
        }
    }
}
