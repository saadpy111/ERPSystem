using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.CreateEmployeeContract
{
    public class CreateEmployeeContractHandler : IRequestHandler<CreateEmployeeContractRequest, CreateEmployeeContractResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateEmployeeContractHandler(
            IEmployeeContractRepository employeeContractRepository,
            IHrAttachmentRepository attachmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService)
        {
            _employeeContractRepository = employeeContractRepository;
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<CreateEmployeeContractResponse> Handle(CreateEmployeeContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeContract = new EmployeeContract
                {
                    EmployeeId = request.EmployeeId,
                    JobId = request.JobId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Salary = request.Salary,
                    ContractType = request.ContractType,
                    Notes = request.Notes,
                      ProbationPeriod = request.ProbationPeriod
                };

                await _employeeContractRepository.AddAsync(employeeContract);
                await _unitOfWork.SaveChangesAsync();

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
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                var employeeContractDto = _mapper.Map<DTOs.EmployeeContractDto>(employeeContract);

                return new CreateEmployeeContractResponse
                {
                    Success = true,
                    Message = "Employee contract created successfully",
                    EmployeeContract = employeeContractDto
                };
            }
            catch (Exception ex)
            {
                return new CreateEmployeeContractResponse
                {
                    Success = false,
                    Message = $"Error creating employee contract: {ex.Message}"
                };
            }
        }
    }
}