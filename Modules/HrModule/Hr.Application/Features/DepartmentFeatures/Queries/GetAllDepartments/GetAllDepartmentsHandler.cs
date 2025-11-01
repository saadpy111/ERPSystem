using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.GetAllDepartments
{
    public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsRequest, GetAllDepartmentsResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetAllDepartmentsHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<GetAllDepartmentsResponse> Handle(GetAllDepartmentsRequest request, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetAllAsync();
            var departmentDtos = _mapper.Map<List<DTOs.DepartmentDto>>(departments);

            return new GetAllDepartmentsResponse
            {
                Departments = departmentDtos
            };
        }
    }
}
