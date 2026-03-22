using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Multitenancy;

namespace Identity.Api.Controllers
{
    /// <summary>
    /// Account and role management for ERP (System) users.
    /// All operations are automatically scoped to UserType.System.
    /// </summary>
    [Route("api/erp")]
    public class ErpAccountsController : AccountManagementBaseController
    {
        protected override UserType ManagedUserType => UserType.System;

        public ErpAccountsController(IMediator mediator,ITenantProvider tenantProvider) : base(mediator , tenantProvider) { }
    }
}
