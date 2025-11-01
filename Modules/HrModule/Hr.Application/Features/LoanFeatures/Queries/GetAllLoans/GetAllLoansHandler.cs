using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.LoanFeatures.GetAllLoans
{
    public class GetAllLoansHandler : IRequestHandler<GetAllLoansRequest, GetAllLoansResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetAllLoansHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<GetAllLoansResponse> Handle(GetAllLoansRequest request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetAllAsync();
            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            return new GetAllLoansResponse
            {
                Loans = loanDtos
            };
        }
    }
}
