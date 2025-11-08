using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System.Linq;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeById
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdRequest, GetEmployeeByIdResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IEmployeeRepository employeeRepository, IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeByIdResponse> Handle(GetEmployeeByIdRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            var employeeDto = _mapper.Map<DTOs.EmployeeDto>(employee);

            // Fetch attachments for this employee
            if (employeeDto != null)
            {
                var attachments = await _attachmentRepository.GetByEntityAsync("Employee", employeeDto.EmployeeId);
                employeeDto.Attachments = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);
            }

            return new GetEmployeeByIdResponse
            {
                Employee = employeeDto
            };
        }
    }
}
