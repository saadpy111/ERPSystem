using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.DepartmentFeatures.CreateDepartment
{
    public class CreateDepartmentRequest : IRequest<CreateDepartmentResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}