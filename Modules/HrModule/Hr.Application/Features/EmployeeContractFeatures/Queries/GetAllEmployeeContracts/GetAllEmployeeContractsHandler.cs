using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetAllEmployeeContracts
{
    public class GetAllEmployeeContractsHandler : IRequestHandler<GetAllEmployeeContractsRequest, GetAllEmployeeContractsResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeeContractsHandler(IEmployeeContractRepository employeeContractRepository, IMapper mapper)
        {
            _employeeContractRepository = employeeContractRepository;
            _mapper = mapper;
        }

        public async Task<GetAllEmployeeContractsResponse> Handle(GetAllEmployeeContractsRequest request, CancellationToken cancellationToken)
        {
            var employeeContracts = await _employeeContractRepository.GetAllAsync();
            var employeeContractDtos = _mapper.Map<IEnumerable<DTOs.EmployeeContractDto>>(employeeContracts);

            return new GetAllEmployeeContractsResponse
            {
                EmployeeContracts = employeeContractDtos
            };
        }
    }
}