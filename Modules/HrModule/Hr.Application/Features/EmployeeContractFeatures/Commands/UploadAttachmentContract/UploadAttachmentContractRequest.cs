using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.UploadAttachmentContract
{
    public class UploadAttachmentContractRequest : IRequest<UploadAttachmentContractResponse>
    {
        public int EmployeeContractId { get; set; }
        public ICollection<IFormFile> AttachmentFiles { get; set; } = new List<IFormFile>();
    }
}