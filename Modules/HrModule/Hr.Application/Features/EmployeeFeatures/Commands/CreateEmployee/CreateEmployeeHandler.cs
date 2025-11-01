using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeRequest, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateEmployeeResponse> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = new Employee
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    DepartmentId = request.DepartmentId,
                    JobTitle = request.JobTitle,
                    HiringDate = request.HiringDate,
                    BaseSalary = request.BaseSalary,
                    Status = EmployeeStatus.Active
                };

                await _employeeRepository.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

                return new CreateEmployeeResponse
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Employee = employeeDto
                };
            }
            catch (System.Exception ex)
            {
                return new CreateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error creating employee: {ex.Message}"
                };
            }
        }
    }
}
