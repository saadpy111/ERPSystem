using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.UpdateEmployee
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeRequest, UpdateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEmployeeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateEmployeeResponse> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.Id);
                if (employee == null)
                {
                    return new UpdateEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                employee.FullName = request.FullName;
                employee.Email = request.Email;
                employee.PhoneNumber = request.Phone;
                employee.DepartmentId = request.DepartmentId;
                employee.JobTitle = request.Position ?? string.Empty;
                employee.HiringDate = request.HiringDate;
                employee.BaseSalary = request.Salary ?? employee.BaseSalary;
                employee.Status = request.Status;

                _employeeRepository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

                return new UpdateEmployeeResponse
                {
                    Success = true,
                    Message = "Employee updated successfully",
                    Employee = employeeDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error updating employee: {ex.Message}"
                };
            }
        }
    }
}
