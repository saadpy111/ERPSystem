using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.DeleteEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeRequest, DeleteEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.Id);
                if (employee == null)
                {
                    return new DeleteEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                _employeeRepository.Delete(employee);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteEmployeeResponse
                {
                    Success = true,
                    Message = "Employee deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteEmployeeResponse
                {
                    Success = false,
                    Message = $"Error deleting employee: {ex.Message}"
                };
            }
        }
    }
}
