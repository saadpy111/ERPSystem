using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System.Linq;

namespace Hr.Application.Features.EmployeeFeatures.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeRequest, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileService _fileService;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IFileService fileService,
            IHrAttachmentRepository attachmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _fileService = fileService;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateEmployeeResponse> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                #region BaseFileds
                var employee = new Employee
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Status = EmployeeStatus.Active,
                    Gender = Enum.TryParse<Gender>(request.Gender, out var gender) ? gender : Gender.Other,
                    Address = request.Address,
                    ManagerId = request.ManagerId
                };
                #endregion


                #region Handle image file if provided
                if (request.ImageFile != null)
                {
                    var folderPath = $"employeesImages";
                    var imageUrl = await _fileService.SaveFileAsync(request.ImageFile, folderPath);
                    employee.ImageUrl = imageUrl;
              
                }
                await _employeeRepository.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();
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
                var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

                return new CreateEmployeeResponse
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Employee = employeeDto
                };
            }
            catch (System.Exception ex)
            {
                return new CreateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error creating employee: {ex.Message}"
                };
            }
        }
    }
}
