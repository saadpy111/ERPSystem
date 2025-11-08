using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.GetDepartmentById
{
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdRequest, GetDepartmentByIdResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentByIdHandler(IDepartmentRepository departmentRepository, IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetDepartmentByIdResponse> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            var departmentDto = _mapper.Map<DTOs.DepartmentDto>(department);

            // Fetch attachments for this department
            if (departmentDto != null)
            {
                var attachments = await _attachmentRepository.GetByEntityAsync("Department", departmentDto.DepartmentId);
                departmentDto.Attachments = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);
            }

            return new GetDepartmentByIdResponse
            {
                Department = departmentDto
            };
        }
    }
}
