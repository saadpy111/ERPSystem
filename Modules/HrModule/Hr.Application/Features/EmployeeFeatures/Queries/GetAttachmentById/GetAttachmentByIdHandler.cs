using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentById
{
    public class GetAttachmentByIdHandler : IRequestHandler<GetAttachmentByIdRequest, GetAttachmentByIdResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetAttachmentByIdHandler(IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetAttachmentByIdResponse> Handle(GetAttachmentByIdRequest request, CancellationToken cancellationToken)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId);
            var attachmentDto = _mapper.Map<DTOs.HrAttachmentDto>(attachment);

            return new GetAttachmentByIdResponse
            {
                Attachment = attachmentDto
            };
        }
    }
}