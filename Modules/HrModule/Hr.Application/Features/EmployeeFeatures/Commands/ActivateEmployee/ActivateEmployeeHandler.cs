using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.ActivateEmployee
{
    public class ActivateEmployeeHandler : IRequestHandler<ActivateEmployeeRequest, ActivateEmployeeResponse>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateEmployeeHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActivateEmployeeResponse> Handle(ActivateEmployeeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new ActivateEmployeeResponse
                    {
                        Success = false,
                        Message = "الموظف غير موجود"
                    };
                }

                employee.Status = EmployeeStatus.Active;
                _repository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new ActivateEmployeeResponse
                {
                    Success = true,
                    Message = "تم تفعيل الموظف بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ActivateEmployeeResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تفعيل الموظف"
                };
            }
        }
    }
}
