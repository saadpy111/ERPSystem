using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentRequest, CreateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateDepartmentResponse> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var department = new Department
            {
                Name = request.Name,
                Description = request.Description
            };

            await _departmentRepository.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            var departmentDto = _mapper.Map<DTOs.DepartmentDto>(department);

            return new CreateDepartmentResponse
            {
                Success = true,
                Message = "Department created successfully",
                Department = departmentDto
            };
        }
    }
}
