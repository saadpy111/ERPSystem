using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.DeletePayrollComponent
{
    public class DeletePayrollComponentHandler : IRequestHandler<DeletePayrollComponentRequest, DeletePayrollComponentResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePayrollComponentHandler(IPayrollComponentRepository payrollComponentRepository, IUnitOfWork unitOfWork)
        {
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
