using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.TerminateEmployee
{
    public class TerminateEmployeeHandler : IRequestHandler<TerminateEmployeeRequest, TerminateEmployeeResponse>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TerminateEmployeeHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TerminateEmployeeResponse> Handle(TerminateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new TerminateEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                employee.Status = EmployeeStatus.Terminated;
                _repository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new TerminateEmployeeResponse
                {
                    Success = true,
                    Message = "Employee terminated successfully"
                };
            }
            catch (Exception ex)
            {
                return new TerminateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error terminating employee: {ex.Message}"
                };
            }
        }
    }
}
