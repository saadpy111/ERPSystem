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
                        Message = "Department not found"
                    };
                }

                _departmentRepository.Delete(department);
                await _unitOfWork.SaveChangesAsync();

                return new DeleteDepartmentResponse
                {
                    Success = true,
                    Message = "Department deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteDepartmentResponse
                {
                    Success = false,
                    Message = $"Error deleting department: {ex.Message}"
                };
            }
        }
    }
}
