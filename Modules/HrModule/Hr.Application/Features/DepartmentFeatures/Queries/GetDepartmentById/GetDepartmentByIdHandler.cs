using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.GetDepartmentById
{
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdRequest, GetDepartmentByIdResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentByIdHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<GetDepartmentByIdResponse> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            var departmentDto = _mapper.Map<DTOs.DepartmentDto>(department);

            return new GetDepartmentByIdResponse
            {
                Department = departmentDto
            };
        }
    }
}
