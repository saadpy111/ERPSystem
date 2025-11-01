using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanFeatures.DeleteLoan
{
    public class DeleteLoanHandler : IRequestHandler<DeleteLoanRequest, DeleteLoanResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLoanHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteLoanResponse> Handle(DeleteLoanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loan = await _loanRepository.GetByIdAsync(request.Id);
                if (loan == null)
                {
                    return new DeleteLoanResponse
                    {
                        Success = false,
                        Message = "Loan not found"
                    };
                }

                _loanRepository.Delete(loan);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteLoanResponse
                {
                    Success = true,
                    Message = "Loan deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteLoanResponse
                {
                    Success = false,
                    Message = $"Error deleting loan: {ex.Message}"
                };
            }
        }
    }
}
