using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetAllLoanInstallments
{
    public class GetAllLoanInstallmentsHandler : IRequestHandler<GetAllLoanInstallmentsRequest, GetAllLoanInstallmentsResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly IMapper _mapper;

        public GetAllLoanInstallmentsHandler(ILoanInstallmentRepository loanInstallmentRepository, IMapper mapper)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _mapper = mapper;
        }

        public async Task<GetAllLoanInstallmentsResponse> Handle(GetAllLoanInstallmentsRequest request, CancellationToken cancellationToken)
        {
            var loanInstallments = await _loanInstallmentRepository.GetAllAsync();
            var loanInstallmentDtos = _mapper.Map<IEnumerable<LoanInstallmentDto>>(loanInstallments);

            return new GetAllLoanInstallmentsResponse
            {
                LoanInstallments = loanInstallmentDtos
            };
        }
    }
}
