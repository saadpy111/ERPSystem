using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeFeatures.CreateEmployee
{
    public class CreateEmployeeRequest : IRequest<CreateEmployeeResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? ManagerId { get; set; }
        
   
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}
