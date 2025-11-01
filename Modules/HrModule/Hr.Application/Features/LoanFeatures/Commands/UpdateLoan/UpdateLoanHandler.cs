using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanFeatures.UpdateLoan
{
    public class UpdateLoanHandler : IRequestHandler<UpdateLoanRequest, UpdateLoanResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateLoanHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateLoanResponse> Handle(UpdateLoanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loan = await _loanRepository.GetByIdAsync(request.Id);
                if (loan == null)
                {
                    return new UpdateLoanResponse
                    {
                        Success = false,
                        Message = "Loan not found"
                    };
                }

                loan.EmployeeId = request.EmployeeId;
                loan.PrincipalAmount = request.PrincipalAmount;
                loan.MonthlyInstallment = request.MonthlyInstallment;
                loan.TermMonths = request.TermMonths;
                loan.StartDate = request.StartDate;
                loan.Status = request.Status;

                _loanRepository.Update(loan);
                await _unitOfWork.SaveChangesAsync();

                var loanDto = _mapper.Map<DTOs.LoanDto>(loan);

                return new UpdateLoanResponse
                {
                    Success = true,
                    Message = "Loan updated successfully",
                    Loan = loanDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateLoanResponse
                {
                    Success = false,
                    Message = $"Error updating loan: {ex.Message}"
                };
            }
        }
    }
}
