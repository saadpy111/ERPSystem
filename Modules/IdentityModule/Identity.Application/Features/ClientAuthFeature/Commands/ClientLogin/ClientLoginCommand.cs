using MediatR;

namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientLogin
{
    /// <summary>
    /// Command to login a client (storefront customer).
    /// Domain is used to resolve the tenant.
    /// </summary>
    public class ClientLoginCommand : IRequest<ClientLoginResponse>
    {
        /// <summary>
        /// Tenant's storefront domain (e.g., "mystore.com")
        /// </summary>
        public string Domain { get; set; } = string.Empty;
        
        /// <summary>
        /// Customer's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Customer's password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
