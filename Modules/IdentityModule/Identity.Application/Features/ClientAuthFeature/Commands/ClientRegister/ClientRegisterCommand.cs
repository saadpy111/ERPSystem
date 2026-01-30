using MediatR;

namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister
{
    /// <summary>
    /// Command to register a new client (storefront customer) for a tenant.
    /// Domain is used to resolve the tenant.
    /// </summary>
    public class ClientRegisterCommand : IRequest<ClientRegisterResponse>
    {
        
        /// <summary>
        /// Customer's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Customer's password
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Customer's full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional phone number
        /// </summary>
        public string? Phone { get; set; }
    }
}
