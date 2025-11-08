using Hr.Application.DTOs;
using System.Collections.Generic;

namespace Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentsByEmployeeId
{
    public class GetAttachmentsByEmployeeIdResponse
    {
        public IEnumerable<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
    }
}