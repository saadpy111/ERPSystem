using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentRequest, UpdateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDepartmentHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateDepartmentResponse> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department == null)
                {
                    return new UpdateDepartmentResponse
                    {
                        Success = false,
                        Message = "Department not found"
                    };
                }

                department.Name = request.Name;
                department.Description = request.Description;

                _departmentRepository.Update(department);
                await _unitOfWork.SaveChangesAsync();

                var departmentDto = _mapper.Map<DTOs.DepartmentDto>(department);

                return new UpdateDepartmentResponse
                {
                    Success = true,
                    Message = "Department updated successfully",
                    Department = departmentDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateDepartmentResponse
                {
                    Success = false,
                    Message = $"Error updating department: {ex.Message}"
                };
            }
        }
    }
}
