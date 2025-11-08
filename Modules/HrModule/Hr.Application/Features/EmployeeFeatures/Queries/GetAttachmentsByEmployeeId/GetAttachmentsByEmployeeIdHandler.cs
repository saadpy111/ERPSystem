using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentsByEmployeeId
{
    public class GetAttachmentsByEmployeeIdHandler : IRequestHandler<GetAttachmentsByEmployeeIdRequest, GetAttachmentsByEmployeeIdResponse>
    {
        private readonly IHrAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;

        public GetAttachmentsByEmployeeIdHandler(IHrAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<GetAttachmentsByEmployeeIdResponse> Handle(GetAttachmentsByEmployeeIdRequest request, CancellationToken cancellationToken)
        {
           
            var attachments = await _attachmentRepository.GetByEntityAsync("Employee", request.EmployeeId);
            var attachmentDtos = _mapper.Map<IEnumerable<DTOs.HrAttachmentDto>>(attachments);

            return new GetAttachmentsByEmployeeIdResponse
            {
                Attachments = attachmentDtos
            };
        }
    }
}