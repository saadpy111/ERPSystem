using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.CreateSalaryStructure
{
    public class CreateSalaryStructureHandler : IRequestHandler<CreateSalaryStructureRequest, CreateSalaryStructureResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSalaryStructureHandler(
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

        public async Task<CreateSalaryStructureResponse> Handle(CreateSalaryStructureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var salaryStructure = new SalaryStructure
                {
                    Name = request.Name,
                    Code = request.Code,
                    Description = request.Description,
                    Type = request.Type,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow
                };


                // Handle components
                if (request.Components != null && request.Components.Any())
                {
                    foreach (var componentDto in request.Components)
                    {
                      
                            var component = new SalaryStructureComponent
                            {
                                Name = componentDto.Name,
                                Type = componentDto.Type,
                                FixedAmount = componentDto.FixedAmount,
                                Percentage = componentDto.Percentage
                            };

                        salaryStructure.Components.Add(component);
                        
                    }
                }
                await _salaryStructureRepository.AddAsync(salaryStructure);

                await _unitOfWork.SaveChangesAsync();
                // Reload the salary structure with components
                var updatedSalaryStructure = await _salaryStructureRepository.GetByIdAsync(salaryStructure.Id);
                var salaryStructureDto = _mapper.Map<DTOs.SalaryStructureDto>(updatedSalaryStructure);

                return new CreateSalaryStructureResponse
                {
                    Success = true,
                    Message = "Salary structure created successfully",
                    SalaryStructure = salaryStructureDto
                };
            }
            catch (Exception ex)
            {
                return new CreateSalaryStructureResponse
                {
                    Success = false,
                    Message = $"Error creating salary structure: {ex.Message}"
                };
            }
        }
    }
}