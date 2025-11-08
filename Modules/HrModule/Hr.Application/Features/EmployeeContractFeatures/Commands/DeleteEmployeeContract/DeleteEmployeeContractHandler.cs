using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteEmployeeContract
{
    public class DeleteEmployeeContractHandler : IRequestHandler<DeleteEmployeeContractRequest, DeleteEmployeeContractResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeContractHandler(IEmployeeContractRepository employeeContractRepository, IUnitOfWork unitOfWork)
        {
            _employeeContractRepository = employeeContractRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteEmployeeContractResponse> Handle(DeleteEmployeeContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeContract = await _employeeContractRepository.GetByIdAsync(request.Id);
                if (employeeContract == null)
                {
                    return new DeleteEmployeeContractResponse
                    {
                        Success = false,
                        Message = "Employee contract not found"
                    };
                }

                _employeeContractRepository.Delete(employeeContract);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteEmployeeContractResponse
                {
                    Success = true,
                    Message = "Employee contract deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteEmployeeContractResponse
                {
                    Success = false,
                    Message = $"Error deleting employee contract: {ex.Message}"
                };
            }
        }
    }
}