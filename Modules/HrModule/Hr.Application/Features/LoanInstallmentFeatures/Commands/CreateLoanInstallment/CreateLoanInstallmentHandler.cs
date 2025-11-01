using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.CreateLoanInstallment
{
    public class CreateLoanInstallmentHandler : IRequestHandler<CreateLoanInstallmentRequest, CreateLoanInstallmentResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLoanInstallmentHandler(ILoanInstallmentRepository loanInstallmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLoanInstallmentResponse> Handle(CreateLoanInstallmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loanInstallment = new LoanInstallment
                {
                    LoanId = request.LoanId,
                    DueDate = request.DueDate,
                    AmountDue = request.AmountDue,
                    PaymentMethod = request.PaymentMethod,
                    Status = InstallmentStatus.Pending
                };

                await _loanInstallmentRepository.AddAsync(loanInstallment);
                await _unitOfWork.SaveChangesAsync();

                var loanInstallmentDto = _mapper.Map<DTOs.LoanInstallmentDto>(loanInstallment);

                return new CreateLoanInstallmentResponse
                {
                    Success = true,
                    Message = "Loan installment created successfully",
                    LoanInstallment = loanInstallmentDto
                };
            }
            catch (Exception ex)
            {
                return new CreateLoanInstallmentResponse
                {
                    Success = false,
                    Message = $"Error creating loan installment: {ex.Message}"
                };
            }
        }
    }
}
