using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LoanFeatures.CreateLoan
{
    public class CreateLoanHandler : IRequestHandler<CreateLoanRequest, CreateLoanResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLoanHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLoanResponse> Handle(CreateLoanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var loan = new Loan
                {
                    EmployeeId = request.EmployeeId,
                    PrincipalAmount = request.PrincipalAmount,
                    MonthlyInstallment = request.MonthlyInstallment,
                    TermMonths = request.TermMonths,
                    StartDate = request.StartDate,
                    RemainingBalance = request.PrincipalAmount,
                    Status = LoanStatus.Active
                };

                await _loanRepository.AddAsync(loan);
                await _unitOfWork.SaveChangesAsync();

                var loanDto = _mapper.Map<DTOs.LoanDto>(loan);

                return new CreateLoanResponse
                {
                    Success = true,
                    Message = "Loan created successfully",
                    Loan = loanDto
                };
            }
            catch (Exception ex)
            {
                return new CreateLoanResponse
                {
                    Success = false,
                    Message = $"Error creating loan: {ex.Message}"
                };
            }
        }
    }
}
