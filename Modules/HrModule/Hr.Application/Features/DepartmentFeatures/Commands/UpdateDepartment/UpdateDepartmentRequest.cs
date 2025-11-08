using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.DepartmentFeatures.UpdateDepartment
{
    public class UpdateDepartmentRequest : IRequest<UpdateDepartmentResponse>
    {
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}
