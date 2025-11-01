using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.DeletePayrollRecord
{
    public class DeletePayrollRecordHandler : IRequestHandler<DeletePayrollRecordRequest, DeletePayrollRecordResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePayrollRecordHandler(IPayrollRecordRepository payrollRecordRepository, IUnitOfWork unitOfWork)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletePayrollRecordResponse> Handle(DeletePayrollRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollRecord = await _payrollRecordRepository.GetByIdAsync(request.PayrollId);
                if (payrollRecord == null)
                {
                    return new DeletePayrollRecordResponse
                    {
                        Success = false,
                        Message = "Payroll record not found"
                    };
                }

                _payrollRecordRepository.Delete(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                return new DeletePayrollRecordResponse
                {
                    Success = true,
                    Message = "Payroll record deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeletePayrollRecordResponse
                {
                    Success = false,
                    Message = $"Error deleting payroll record: {ex.Message}"
                };
            }
        }
    }
}
