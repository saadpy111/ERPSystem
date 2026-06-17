using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using System;
using System.Threading.Tasks;
using Website.Application.Features.NewsletterFeatures.Commands.AdminDeleteNewsletterSubscriber;
using Website.Application.Features.NewsletterFeatures.Commands.SubscribeNewsletter;
using Website.Application.Features.NewsletterFeatures.Commands.UnsubscribeNewsletter;
using Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscriberById;
using Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscribersPaged;

namespace Website.Api.Controllers
{
    [Route("api/website")]
    [ApiExplorerSettings(GroupName = "Website")]
    [ApiController]
    [Authorize]
    public class NewsletterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsletterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("newsletter/subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeNewsletterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("admin/newsletter")]
        [HasPermission(WebsitePermissions.NewsletterView)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null)
        {
            var result = await _mediator.Send(new GetNewsletterSubscribersPagedQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                IsActive = isActive
            });

            return Ok(result);
        }

        [HttpGet("admin/newsletter/{id}")]
        [HasPermission(WebsitePermissions.NewsletterView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetNewsletterSubscriberByIdQuery { Id = id });
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("admin/newsletter/{id}")]
        [HasPermission(WebsitePermissions.NewsletterDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new AdminDeleteNewsletterSubscriberCommand { Id = id });
            if (!result)
                return NotFound();

            return Ok(new { success = true, message = "Subscriber deleted successfully" });
        }
    }
}
