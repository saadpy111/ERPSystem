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
                        Message = "لم يتم العثور على قسط القرض"
                    };
                }

                _loanInstallmentRepository.Delete(loanInstallment);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteLoanInstallmentResponse
                {
                    Success = true,
                    Message = "تم حذف قسط القرض بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteLoanInstallmentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف قسط القرض"
                };
            }
        }
    }
}
