using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment
{
    public class UpdateLoanInstallmentHandler : IRequestHandler<UpdateLoanInstallmentRequest, UpdateLoanInstallmentResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLoanInstallmentHandler(ILoanInstallmentRepository loanInstallmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateLoanInstallmentResponse> Handle(UpdateLoanInstallmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loanInstallment = await _loanInstallmentRepository.GetByIdAsync(request.InstallmentId);
                if (loanInstallment == null)
                {
                    return new UpdateLoanInstallmentResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على قسط القرض"
                    };
                }

                loanInstallment.LoanId = request.LoanId;
                loanInstallment.DueDate = request.DueDate;
                loanInstallment.AmountDue = request.AmountDue;
                loanInstallment.PaymentDate = request.PaymentDate;
                loanInstallment.PaymentMethod = request.PaymentMethod;
                loanInstallment.Status = request.Status;

                _loanInstallmentRepository.Update(loanInstallment);
                await _unitOfWork.SaveChangesAsync();

                var loanInstallmentDto = _mapper.Map<DTOs.LoanInstallmentDto>(loanInstallment);

                return new UpdateLoanInstallmentResponse
                {
                    Success = true,
                    Message = "تم تحديث قسط القرض بنجاح",
                    LoanInstallment = loanInstallmentDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateLoanInstallmentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تحديث قسط القرض"
                };
            }
        }
    }
}
