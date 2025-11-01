using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.GetAllPayrollComponents
{
    public class GetAllPayrollComponentsHandler : IRequestHandler<GetAllPayrollComponentsRequest, GetAllPayrollComponentsResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IMapper _mapper;

        public GetAllPayrollComponentsHandler(IPayrollComponentRepository payrollComponentRepository, IMapper mapper)
        {
            _payrollComponentRepository = payrollComponentRepository;
            _mapper = mapper;
        }

        public async Task<GetAllPayrollComponentsResponse> Handle(GetAllPayrollComponentsRequest request, CancellationToken cancellationToken)
        {
            var payrollComponents = await _payrollComponentRepository.GetAllAsync();
            var payrollComponentDtos = _mapper.Map<IEnumerable<PayrollComponentDto>>(payrollComponents);

            return new GetAllPayrollComponentsResponse
            {
                PayrollComponents = payrollComponentDtos
            };
        }
    }
}
