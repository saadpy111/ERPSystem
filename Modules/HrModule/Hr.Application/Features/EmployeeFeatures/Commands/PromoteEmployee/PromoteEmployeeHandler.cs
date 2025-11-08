using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.PromoteEmployee
{
    public class PromoteEmployeeHandler : IRequestHandler<PromoteEmployeeRequest, PromoteEmployeeResponse>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PromoteEmployeeHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PromoteEmployeeResponse> Handle(PromoteEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new PromoteEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                // Update employee details
                
                _repository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                // Reload to get department
                employee = await _repository.GetByIdAsync(request.EmployeeId);

                return new PromoteEmployeeResponse
                {
                    Success = true,
                    Message = "Employee promoted successfully",
                    Employee = new EmployeeDto
                    {
                        EmployeeId = employee!.EmployeeId,
                        FullName = employee.FullName,
                        Email = employee.Email,
                        PhoneNumber = employee.PhoneNumber,
                        DateOfBirth = employee.DateOfBirth,
                        Status = employee.Status.ToString(),
                        CreatedAt = employee.CreatedAt,
                        UpdatedAt = employee.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new PromoteEmployeeResponse
                {
                    Success = false,
                    Message = $"Error promoting employee: {ex.Message}"
                };
            }
        }
    }
}
