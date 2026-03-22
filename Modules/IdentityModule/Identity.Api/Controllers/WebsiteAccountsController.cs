using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Multitenancy;

namespace Identity.Api.Controllers
{
    /// <summary>
    /// Account and role management for Website (Client) users.
    /// All operations are automatically scoped to UserType.Client.
    /// </summary>
    [Route("api/website")]
    public class WebsiteAccountsController : AccountManagementBaseController
    {
        protected override UserType ManagedUserType => UserType.Client;

        public WebsiteAccountsController(IMediator mediator , ITenantProvider tenantProvider) : base(mediator , tenantProvider) { }
    }
}
