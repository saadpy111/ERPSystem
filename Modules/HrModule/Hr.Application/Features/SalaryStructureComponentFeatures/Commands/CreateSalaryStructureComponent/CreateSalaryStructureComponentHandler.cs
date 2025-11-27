using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.CreateSalaryStructureComponent
{
    public class CreateSalaryStructureComponentHandler : IRequestHandler<CreateSalaryStructureComponentRequest, CreateSalaryStructureComponentResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSalaryStructureComponentHandler(
            ISalaryStructureComponentRepository salaryStructureComponentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateSalaryStructureComponentResponse> Handle(CreateSalaryStructureComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
               

                var salaryStructureComponent = new SalaryStructureComponent
                {
                    SalaryStructureId = request.SalaryStructureId,
                    Name = request.Name,
                    Type = request.Type,
                    FixedAmount = request.FixedAmount,
                    Percentage = request.Percentage,
                    CreatedAt = DateTime.UtcNow
                };

                await _salaryStructureComponentRepository.AddAsync(salaryStructureComponent);
                await _unitOfWork.SaveChangesAsync();

                var salaryStructureComponentDto = _mapper.Map<DTOs.SalaryStructureComponentDto>(salaryStructureComponent);

                return new CreateSalaryStructureComponentResponse
                {
                    Success = true,
                    Message = "تم إنشاء مكون هيكل الرواتب بنجاح",
                    SalaryStructureComponent = salaryStructureComponentDto
                };
            }
            catch (Exception ex)
            {
                return new CreateSalaryStructureComponentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء مكون هيكل الرواتب"
                };
            }
        }
    }
}