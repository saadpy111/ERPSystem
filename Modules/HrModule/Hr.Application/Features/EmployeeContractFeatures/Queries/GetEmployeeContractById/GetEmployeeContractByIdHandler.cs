using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractById
{
    public class GetEmployeeContractByIdHandler : IRequestHandler<GetEmployeeContractByIdRequest, GetEmployeeContractByIdResponse>
    {
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetEmployeeContractByIdHandler(IEmployeeContractRepository employeeContractRepository, IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _employeeContractRepository = employeeContractRepository;
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeContractByIdResponse> Handle(GetEmployeeContractByIdRequest request, CancellationToken cancellationToken)
        {
            var employeeContract = await _employeeContractRepository.GetByIdAsync(request.Id);
            var employeeContractDto = _mapper.Map<DTOs.EmployeeContractDto>(employeeContract);

            // Fetch attachments for this employee contract
            if (employeeContractDto != null)
            {
                var attachments = await _attachmentRepository.GetByEntityAsync("EmployeeContract", employeeContractDto.Id);
                employeeContractDto.Attachments = _mapper.Map<ICollection<DTOs.HrAttachmentDto>>(attachments);
            }

            return new GetEmployeeContractByIdResponse
            {
                EmployeeContract = employeeContractDto
            };
        }
    }
}