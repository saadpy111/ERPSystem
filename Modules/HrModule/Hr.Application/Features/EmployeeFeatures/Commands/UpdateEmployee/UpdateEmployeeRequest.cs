using MediatR;
using Hr.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeFeatures.UpdateEmployee
{
    public class UpdateEmployeeRequest : IRequest<UpdateEmployeeResponse>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public EmployeeStatus Status { get; set; }
        

        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? ManagerId { get; set; }
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}
