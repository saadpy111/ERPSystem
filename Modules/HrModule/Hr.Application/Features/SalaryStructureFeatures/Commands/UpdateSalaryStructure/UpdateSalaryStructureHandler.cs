using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure
{
    public class UpdateSalaryStructureHandler : IRequestHandler<UpdateSalaryStructureRequest, UpdateSalaryStructureResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSalaryStructureHandler(
            ISalaryStructureRepository salaryStructureRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _salaryStructureRepository = salaryStructureRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateSalaryStructureResponse> Handle(UpdateSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var salaryStructure = await _salaryStructureRepository.GetByIdAsync(request.Id);
                if (salaryStructure == null)
                {
                    return new UpdateSalaryStructureResponse
                    {
                        Success = false,
                        Message = "Salary structure not found"
                    };
                }

 

                salaryStructure.Name = request.Name;
                salaryStructure.Code = request.Code;
                salaryStructure.Description = request.Description;
                salaryStructure.Type = request.Type;
                salaryStructure.IsActive = request.IsActive;
                salaryStructure.UpdatedAt = DateTime.UtcNow;

                _salaryStructureRepository.Update(salaryStructure);
                await _unitOfWork.SaveChangesAsync();

                var salaryStructureDto = _mapper.Map<DTOs.SalaryStructureDto>(salaryStructure);

                return new UpdateSalaryStructureResponse
                {
                    Success = true,
                    Message = "Salary structure updated successfully",
                    SalaryStructure = salaryStructureDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateSalaryStructureResponse
                {
                    Success = false,
                    Message = $"Error updating salary structure: {ex.Message}"
                };
            }
        }
    }
}