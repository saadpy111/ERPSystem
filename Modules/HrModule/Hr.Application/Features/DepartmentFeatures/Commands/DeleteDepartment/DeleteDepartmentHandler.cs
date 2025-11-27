using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.DeleteDepartment
{
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentRequest, DeleteDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteDepartmentResponse> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department == null)
                {
                    return new DeleteDepartmentResponse
                    {
                        Success = false,
                        Message = "القسم غير موجود"
                    };
                }

                _departmentRepository.Delete(department);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteDepartmentResponse
                {
                    Success = true,
                    Message = "تم حذف القسم بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new DeleteDepartmentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء حذف القسم"
                };
            }
        }
    }
}
