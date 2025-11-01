using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.DeleteLoanInstallment
{
    public class DeleteLoanInstallmentHandler : IRequestHandler<DeleteLoanInstallmentRequest, DeleteLoanInstallmentResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLoanInstallmentHandler(ILoanInstallmentRepository loanInstallmentRepository, IUnitOfWork unitOfWork)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteLoanInstallmentResponse> Handle(DeleteLoanInstallmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loanInstallment = await _loanInstallmentRepository.GetByIdAsync(request.InstallmentId);
                if (loanInstallment == null)
                {
                    return new DeleteLoanInstallmentResponse
                    {
                        Success = false,
                        Message = "Loan installment not found"
                    };
                }

                _loanInstallmentRepository.Delete(loanInstallment);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteLoanInstallmentResponse
                {
                    Success = true,
                    Message = "Loan installment deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteLoanInstallmentResponse
                {
                    Success = false,
                    Message = $"Error deleting loan installment: {ex.Message}"
                };
            }
        }
    }
}
