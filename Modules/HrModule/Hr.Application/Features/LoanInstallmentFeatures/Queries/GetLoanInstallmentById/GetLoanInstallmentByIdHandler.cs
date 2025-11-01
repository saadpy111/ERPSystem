using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentById
{
    public class GetLoanInstallmentByIdHandler : IRequestHandler<GetLoanInstallmentByIdRequest, GetLoanInstallmentByIdResponse>
    {
        private readonly ILoanInstallmentRepository _loanInstallmentRepository;
        private readonly IMapper _mapper;

        public GetLoanInstallmentByIdHandler(ILoanInstallmentRepository loanInstallmentRepository, IMapper mapper)
        {
            _loanInstallmentRepository = loanInstallmentRepository;
            _mapper = mapper;
        }

        public async Task<GetLoanInstallmentByIdResponse> Handle(GetLoanInstallmentByIdRequest request, CancellationToken cancellationToken)
        {
            var loanInstallment = await _loanInstallmentRepository.GetByIdAsync(request.Id);
            var loanInstallmentDto = _mapper.Map<DTOs.LoanInstallmentDto>(loanInstallment);

            return new GetLoanInstallmentByIdResponse
            {
                LoanInstallment = loanInstallmentDto
            };
        }
    }
}
