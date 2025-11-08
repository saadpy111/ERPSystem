using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsByEmployeeId
{
    public class GetEmployeeContractsByEmployeeIdHandler : IRequestHandler<GetEmployeeContractsByEmployeeIdRequest, GetEmployeeContractsByEmployeeIdResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IMapper _mapper;

        public GetEmployeeContractsByEmployeeIdHandler(IEmployeeContractRepository employeeContractRepository, IMapper mapper)
        {
            _employeeContractRepository = employeeContractRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeContractsByEmployeeIdResponse> Handle(GetEmployeeContractsByEmployeeIdRequest request, CancellationToken cancellationToken)
        {
            var employeeContracts = (await _employeeContractRepository.GetAllAsync())
                .Where(ec => ec.EmployeeId == request.EmployeeId)
                .ToList();

            var employeeContractDtos = _mapper.Map<IEnumerable<DTOs.EmployeeContractDto>>(employeeContracts);

            return new GetEmployeeContractsByEmployeeIdResponse
            {
                EmployeeContracts = employeeContractDtos
            };
        }
    }
}