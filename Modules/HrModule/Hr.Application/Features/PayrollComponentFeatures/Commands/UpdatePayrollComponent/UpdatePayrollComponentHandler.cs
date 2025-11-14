using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent;
using Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent
{
    public class UpdatePayrollComponentHandler : IRequestHandler<UpdatePayrollComponentRequest, UpdatePayrollComponentResponse>
    {
        private readonly IMediator _mediator;
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePayrollComponentHandler(IMediator mediator,IPayrollComponentRepository payrollComponentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
             _mediator = mediator;
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
                payrollComponent.Percentage = request.Percentage;
                payrollComponent.FixedAmount = request.FixedAmount;
                   
                _payrollComponentRepository.Update(payrollComponent);
                await _unitOfWork.SaveChangesAsync();

                var recalculateRequest = new CalculatePayrollRequest
                {
                    PayrollRecordId = request.PayrollRecordId
                };

                var recalculateResult = await _mediator.Send(recalculateRequest, cancellationToken);

                if (!recalculateResult.Success)
                {
                    return new UpdatePayrollComponentResponse
                    {
                        Success = false,
                        Message = recalculateResult.Message
                    };
                }


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
