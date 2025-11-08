using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetActiveEmployees
{
    public class GetActiveEmployeesHandler : IRequestHandler<GetActiveEmployeesRequest, GetActiveEmployeesResponse>
    {
        private readonly IEmployeeRepository _repository;

        public GetActiveEmployeesHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetActiveEmployeesResponse> Handle(GetActiveEmployeesRequest request, CancellationToken cancellationToken)
        {
            var employees = (await _repository.GetAllAsync())
                .Where(e => e.Status == EmployeeStatus.Active)
                .OrderBy(e => e.FullName)
                .ToList();

            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Status = e.Status.ToString()
            });

            return new GetActiveEmployeesResponse
            {
                Employees = employeeDtos
            };
        }
    }
}
