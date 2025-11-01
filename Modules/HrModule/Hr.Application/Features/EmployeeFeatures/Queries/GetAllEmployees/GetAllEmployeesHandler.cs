using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetAllEmployees
{
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesRequest, GetAllEmployeesResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeesHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetAllEmployeesResponse> Handle(GetAllEmployeesRequest request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new GetAllEmployeesResponse
            {
                Employees = employeeDtos
            };
        }
    }
}
