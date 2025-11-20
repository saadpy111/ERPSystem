using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.Queries.GetDepartmentTree
{
    public class GetDepartmentTreeHandler : IRequestHandler<GetDepartmentTreeRequest, GetDepartmentTreeResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentTreeHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<GetDepartmentTreeResponse> Handle(GetDepartmentTreeRequest request, CancellationToken cancellationToken)
        {
            // Get department tree directly from repository for better performance
            var departments = await _departmentRepository.GetDepartmentTreeAsync();
            
            // Map to DTOs
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            return new GetDepartmentTreeResponse
            {
                Departments = departmentDtos
            };
        }
    }
}