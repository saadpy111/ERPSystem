using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetActiveEmployeeContracts
{
    public class GetActiveEmployeeContractsHandler : IRequestHandler<GetActiveEmployeeContractsRequest, GetActiveEmployeeContractsResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IMapper _mapper;

        public GetActiveEmployeeContractsHandler(IEmployeeContractRepository employeeContractRepository, IMapper mapper)
        {
            _employeeContractRepository = employeeContractRepository;
            _mapper = mapper;
        }

        public async Task<GetActiveEmployeeContractsResponse> Handle(GetActiveEmployeeContractsRequest request, CancellationToken cancellationToken)
        {
            var employeeContracts = (await _employeeContractRepository.GetAllAsync())
                .Where(ec => ec.IsActive)
                .ToList();

            var employeeContractDtos = _mapper.Map<IEnumerable<DTOs.EmployeeContractDto>>(employeeContracts);

            return new GetActiveEmployeeContractsResponse
            {
                EmployeeContracts = employeeContractDtos
            };
        }
    }
}