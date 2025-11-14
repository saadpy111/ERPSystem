using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.UpdateSalaryStructureComponent
{
    public class UpdateSalaryStructureComponentHandler : IRequestHandler<UpdateSalaryStructureComponentRequest, UpdateSalaryStructureComponentResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSalaryStructureComponentHandler(
            ISalaryStructureComponentRepository salaryStructureComponentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateSalaryStructureComponentResponse> Handle(UpdateSalaryStructureComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var salaryStructureComponent = await _salaryStructureComponentRepository.GetByIdAsync(request.Id);
                if (salaryStructureComponent == null)
                {
                    return new UpdateSalaryStructureComponentResponse
                    {
                        Success = false,
                        Message = "Salary structure component not found"
                    };
                }

           

                salaryStructureComponent.SalaryStructureId = request.SalaryStructureId;
                salaryStructureComponent.Name = request.Name;
                salaryStructureComponent.Type = request.Type;
                salaryStructureComponent.FixedAmount = request.FixedAmount;
                salaryStructureComponent.Percentage = request.Percentage;
                salaryStructureComponent.UpdatedAt = DateTime.UtcNow;

                _salaryStructureComponentRepository.Update(salaryStructureComponent);
                await _unitOfWork.SaveChangesAsync();

                var salaryStructureComponentDto = _mapper.Map<DTOs.SalaryStructureComponentDto>(salaryStructureComponent);

                return new UpdateSalaryStructureComponentResponse
                {
                    Success = true,
                    Message = "Salary structure component updated successfully",
                    SalaryStructureComponent = salaryStructureComponentDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateSalaryStructureComponentResponse
                {
                    Success = false,
                    Message = $"Error updating salary structure component: {ex.Message}"
                };
            }
        }
    }
}