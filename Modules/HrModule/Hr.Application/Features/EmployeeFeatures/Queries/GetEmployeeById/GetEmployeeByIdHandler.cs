using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeById
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdRequest, GetEmployeeByIdResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeByIdResponse> Handle(GetEmployeeByIdRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

            return new GetEmployeeByIdResponse
            {
                Employee = employeeDto
            };
        }
    }
}
