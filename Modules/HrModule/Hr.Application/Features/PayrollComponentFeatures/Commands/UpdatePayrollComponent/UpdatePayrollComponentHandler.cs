using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent
{
    public class UpdatePayrollComponentHandler : IRequestHandler<UpdatePayrollComponentRequest, UpdatePayrollComponentResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePayrollComponentHandler(IPayrollComponentRepository payrollComponentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _payrollComponentRepository = payrollComponentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdatePayrollComponentResponse> Handle(UpdatePayrollComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollComponent = await _payrollComponentRepository.GetByIdAsync(request.ComponentId);
                if (payrollComponent == null)
                {
                    return new UpdatePayrollComponentResponse
                    {
                        Success = false,
                        Message = "Payroll component not found"
                    };
                }

                payrollComponent.PayrollRecordId = request.PayrollRecordId;
                payrollComponent.Name = request.Name;
                payrollComponent.ComponentType = request.ComponentType;
                payrollComponent.Amount = request.Amount;

                _payrollComponentRepository.Update(payrollComponent);
                await _unitOfWork.SaveChangesAsync();

                var payrollComponentDto = _mapper.Map<DTOs.PayrollComponentDto>(payrollComponent);

                return new UpdatePayrollComponentResponse
                {
                    Success = true,
                    Message = "Payroll component updated successfully",
                    PayrollComponent = payrollComponentDto
                };
            }
            catch (Exception ex)
            {
                return new UpdatePayrollComponentResponse
                {
                    Success = false,
                    Message = $"Error updating payroll component: {ex.Message}"
                };
            }
        }
    }
}
