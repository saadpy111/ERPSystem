using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanFeatures.GetLoanById
{
    public class GetLoanByIdHandler : IRequestHandler<GetLoanByIdRequest, GetLoanByIdResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetLoanByIdHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<GetLoanByIdResponse> Handle(GetLoanByIdRequest request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(request.Id);
            var loanDto = _mapper.Map<DTOs.LoanDto>(loan);

            return new GetLoanByIdResponse
            {
                Loan = loanDto
            };
        }
    }
}
