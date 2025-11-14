using AutoMapper;
using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.UpdateEmployeeContract
{
    public class UpdateEmployeeContractHandler : IRequestHandler<UpdateEmployeeContractRequest, UpdateEmployeeContractResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateEmployeeContractHandler(
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

        public async Task<UpdateEmployeeContractResponse> Handle(UpdateEmployeeContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeContract = await _employeeContractRepository.GetByIdAsync(request.Id);
                if (employeeContract == null)
                {
                    return new UpdateEmployeeContractResponse
                    {
                        Success = false,
                        Message = "Employee contract not found"
                    };
                }

                employeeContract.EmployeeId = request.EmployeeId;
                employeeContract.JobId = request.JobId;
                employeeContract.SalaryStructureId = request.SalaryStructureId;
                employeeContract.StartDate = request.StartDate;
                employeeContract.EndDate = request.EndDate;
                employeeContract.Salary = request.Salary;
                employeeContract.ContractType = request.ContractType;
                employeeContract.ProbationPeriod = request.ProbationPeriod;
                employeeContract.Notes = request.Notes;
                employeeContract.IsActive = request.IsActive;

                _employeeContractRepository.Update(employeeContract);
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

                return new UpdateEmployeeContractResponse
                {
                    Success = true,
                    Message = "Employee contract updated successfully",
                    EmployeeContract = employeeContractDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateEmployeeContractResponse
                {
                    Success = false,
                    Message = $"Error updating employee contract: {ex.Message}"
                };
            }
        }
    }
}