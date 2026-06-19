using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {
        // Populated from JWT in controller
        public string TenantId { get; set; } = string.Empty;

        /// <summary>Enforced by the controller — System or Client.</summary>
        public UserType UserType { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
