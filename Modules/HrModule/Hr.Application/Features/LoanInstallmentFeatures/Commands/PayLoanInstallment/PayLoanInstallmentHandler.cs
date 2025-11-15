using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.LoanInstallmentFeatures.PayLoanInstallment
{
    public class PayLoanInstallmentHandler : IRequestHandler<PayLoanInstallmentRequest, PayLoanInstallmentResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PayLoanInstallmentHandler(
            ILoanInstallmentRepository loanInstallmentRepository,
            ILoanRepository loanRepository,
            IUnitOfWork unitOfWork)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PayLoanInstallmentResponse> Handle(PayLoanInstallmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the installment
                var installment = await _loanInstallmentRepository.GetByIdAsync(request.InstallmentId);
                if (installment == null)
                {
                    return new PayLoanInstallmentResponse
                    {
                        Success = false,
                        Message = "Loan installment not found"
                    };
                }

                // Check if installment is already paid
                if (installment.Status == InstallmentStatus.Paid)
                {
                    return new PayLoanInstallmentResponse
                    {
                        Success = false,
                        Message = "Loan installment is already paid"
                    };
                }

                // Update installment status to paid
                installment.Status = InstallmentStatus.Paid;
                installment.PaymentDate = request.PaymentDate;
                installment.PaymentMethod = request.PaymentMethod;

                // Get the associated loan
                var loan = await _loanRepository.GetByIdAsync(installment.LoanId);
                if (loan == null)
                {
                    return new PayLoanInstallmentResponse
                    {
                        Success = false,
                        Message = "Associated loan not found"
                    };
                }

                // Subtract the amount from the remaining balance
                loan.RemainingBalance -= installment.AmountDue;

                // If remaining balance is zero or negative, mark loan as paid
                if (loan.RemainingBalance <= 0)
                {
                    loan.RemainingBalance = 0;
                    loan.Status = LoanStatus.Paid;
                }

                // Update both entities
                _loanInstallmentRepository.Update(installment);
                _loanRepository.Update(loan);

                // Save changes
                await _unitOfWork.SaveChangesAsync();

                return new PayLoanInstallmentResponse
                {
                    Success = true,
                    Message = "Loan installment paid successfully",
                    InstallmentId = installment.InstallmentId
                };
            }
            catch (Exception ex)
            {
                return new PayLoanInstallmentResponse
                {
                    Success = false,
                    Message = $"Error paying loan installment: {ex.Message}"
                };
            }
        }
    }
}