using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentById
{
    public class GetPayrollComponentByIdHandler : IRequestHandler<GetPayrollComponentByIdRequest, GetPayrollComponentByIdResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IMapper _mapper;

        public GetPayrollComponentByIdHandler(IPayrollComponentRepository payrollComponentRepository, IMapper mapper)
        {
            _payrollComponentRepository = payrollComponentRepository;
            _mapper = mapper;
        }

        public async Task<GetPayrollComponentByIdResponse> Handle(GetPayrollComponentByIdRequest request, CancellationToken cancellationToken)
        {
            var payrollComponent = await _payrollComponentRepository.GetByIdAsync(request.Id);
            var payrollComponentDto = _mapper.Map<DTOs.PayrollComponentDto>(payrollComponent);

            return new GetPayrollComponentByIdResponse
            {
                PayrollComponent = payrollComponentDto
            };
        }
    }
}
