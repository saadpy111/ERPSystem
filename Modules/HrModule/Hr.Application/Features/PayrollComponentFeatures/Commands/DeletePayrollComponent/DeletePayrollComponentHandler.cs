using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent;
using Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.DeletePayrollComponent
{
    public class DeletePayrollComponentHandler : IRequestHandler<DeletePayrollComponentRequest, DeletePayrollComponentResponse>
    {
        private readonly IMediator _mediator;
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePayrollComponentHandler(IMediator mediator,IPayrollComponentRepository payrollComponentRepository, IUnitOfWork unitOfWork)
        {
              _mediator = mediator;
            _payrollComponentRepository = payrollComponentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletePayrollComponentResponse> Handle(DeletePayrollComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollComponent = await _payrollComponentRepository.GetByIdAsync(request.ComponentId);
                if (payrollComponent == null)
                {
                    return new DeletePayrollComponentResponse
                    {
                        Success = false,
                        Message = "Payroll component not found"
                    };
                }

                _payrollComponentRepository.Delete(payrollComponent);
                await _unitOfWork.SaveChangesAsync();


                var recalculateRequest = new CalculatePayrollRequest
                {
                    PayrollRecordId = payrollComponent.PayrollRecordId
                };
                var recalculateResult = await _mediator.Send(recalculateRequest, cancellationToken);

                if (!recalculateResult.Success)
                {
                    return new DeletePayrollComponentResponse
                    {
                        Success = false,
                        Message = recalculateResult.Message
                    };
                }

                return new DeletePayrollComponentResponse
                {
                    Success = true,
                    Message = "Payroll component deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeletePayrollComponentResponse
                {
                    Success = false,
                    Message = $"Error deleting payroll component: {ex.Message}"
                };
            }
        }
    }
}
