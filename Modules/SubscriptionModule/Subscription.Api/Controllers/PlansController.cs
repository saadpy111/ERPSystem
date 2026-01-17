using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subscription.Application.Features.Plans.Queries.GetAvailablePlans;
using System.Threading.Tasks;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscription/plans")]
    [ApiExplorerSettings(GroupName = "Subscription")]

    public class PlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available subscription plans (PUBLIC - No auth required)
        /// </summary>
        /// <param name="currency">Optional currency filter (USD, EGP, etc.)</param>
        /// <param name="includeHidden">Include hidden plans (default: false)</param>
        [HttpGet]
        [ProducesResponseType(typeof(GetAvailablePlansResponse), 200)]
        public async Task<IActionResult> GetAvailablePlans(
            [FromQuery] string? currency = null,
            [FromQuery] bool includeHidden = false)
        {
            var query = new GetAvailablePlansQuery
            {
                CurrencyCode = currency,
                IncludeHidden = includeHidden
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
