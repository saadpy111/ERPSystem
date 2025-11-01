using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent
{
    public class CreatePayrollComponentHandler : IRequestHandler<CreatePayrollComponentRequest, CreatePayrollComponentResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePayrollComponentHandler(IPayrollComponentRepository payrollComponentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _payrollComponentRepository = payrollComponentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreatePayrollComponentResponse> Handle(CreatePayrollComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollComponent = new PayrollComponent
                {
                    PayrollRecordId = request.PayrollRecordId,
                    Name = request.Name,
                    ComponentType = request.ComponentType,
                    Amount = request.Amount
                };

                await _payrollComponentRepository.AddAsync(payrollComponent);
                await _unitOfWork.SaveChangesAsync();

                var payrollComponentDto = _mapper.Map<DTOs.PayrollComponentDto>(payrollComponent);

                return new CreatePayrollComponentResponse
                {
                    Success = true,
                    Message = "Payroll component created successfully",
                    PayrollComponent = payrollComponentDto
                };
            }
            catch (Exception ex)
            {
                return new CreatePayrollComponentResponse
                {
                    Success = false,
                    Message = $"Error creating payroll component: {ex.Message}"
                };
            }
        }
    }
}
