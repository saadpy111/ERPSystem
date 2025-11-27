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
                        Message = "الموظف غير موجود"
                    };
                }

                _employeeRepository.Delete(employee);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteEmployeeResponse
                {
                    Success = true,
                    Message = "تم حذف الموظف بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteEmployeeResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف الموظف"
                };
            }
        }
    }
}
