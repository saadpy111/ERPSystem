using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure
{
    public class UpdateSalaryStructureHandler : IRequestHandler<UpdateSalaryStructureRequest, UpdateSalaryStructureResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSalaryStructureHandler(
            ISalaryStructureRepository salaryStructureRepository,
            ISalaryStructureComponentRepository salaryStructureComponentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _salaryStructureRepository = salaryStructureRepository;
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
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
                        Message = "لم يتم العثور على هيكل الرواتب"
                    };
                }

                salaryStructure.Name = request.Name;
                salaryStructure.Code = request.Code;
                salaryStructure.Description = request.Description;
                salaryStructure.Type = request.Type;
                salaryStructure.IsActive = request.IsActive;
                salaryStructure.UpdatedAt = DateTime.UtcNow;

                if (salaryStructure.Components != null)
                {
                    var existingComponents = salaryStructure.Components;
                    var existingComponentIds = existingComponents.Select(c => c.Id).ToList();
                    var requestComponentIds = request.Components.Where(c => c.Id > 0).Select(c => c.Id).ToList();

                    var componentsToDelete = existingComponents.Where(c => !requestComponentIds.Contains(c.Id)).ToList();
                    foreach (var component in componentsToDelete)
                    {
                        salaryStructure.Components.Remove(component);
                    }

                    // Update or add components
                    foreach (var componentDto in request.Components)
                    {

                        if (componentDto.Id > 0 && existingComponentIds.Contains(componentDto.Id))
                        {
                            // Update existing component
                            var existingComponent = existingComponents.FirstOrDefault(c => c.Id == componentDto.Id);
                            if (existingComponent != null)
                            {
                                existingComponent.Name = componentDto.Name;
                                existingComponent.Type = componentDto.Type;
                                existingComponent.FixedAmount = componentDto.FixedAmount;
                                existingComponent.Percentage = componentDto.Percentage;
                            }
                        }
                        else
                        {
                            // Add new component
                            var newComponent = new SalaryStructureComponent
                            {
                                SalaryStructureId = salaryStructure.Id,
                                Name = componentDto.Name,
                                Type = componentDto.Type,
                                FixedAmount = componentDto.FixedAmount,
                                Percentage = componentDto.Percentage
                            };
                            salaryStructure.Components.Add(newComponent);
                        }

                    }
                }
                _salaryStructureRepository.Update(salaryStructure);
                await _unitOfWork.SaveChangesAsync();

                // Reload the salary structure with components
                var updatedSalaryStructure = await _salaryStructureRepository.GetByIdAsync(salaryStructure.Id);
                var salaryStructureDto = _mapper.Map<DTOs.SalaryStructureDto>(updatedSalaryStructure);

                return new UpdateSalaryStructureResponse
                {
                    Success = true,
                    Message = "تم تحديث هيكل الرواتب بنجاح",
                    SalaryStructure = salaryStructureDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateSalaryStructureResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تحديث هيكل الرواتب"
                };
            }
        }
    }
}