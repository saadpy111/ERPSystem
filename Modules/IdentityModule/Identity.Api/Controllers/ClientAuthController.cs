using Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    /// <summary>
    /// Authentication endpoints for storefront customers (clients).
    /// Uses domain to resolve the tenant.
    /// </summary>
    [ApiController]
    [Route("api/client")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [AllowAnonymous]
    public class ClientAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new customer for a tenant's storefront.
        /// </summary>
        /// <param name="command">Registration details including domain</param>
        /// <returns>User info and JWT token</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ClientRegisterResponse), 200)]
        [ProducesResponseType(typeof(ClientRegisterResponse), 400)]
        public async Task<IActionResult> Register([FromBody] ClientRegisterCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Login a customer to a tenant's storefront.
        /// </summary>
        /// <param name="command">Login credentials including domain</param>
        /// <returns>User info and JWT token</returns>
        //[HttpPost("login")]
        //[ProducesResponseType(typeof(ClientLoginResponse), 200)]
        //[ProducesResponseType(typeof(ClientLoginResponse), 400)]
        //public async Task<IActionResult> Login([FromBody] ClientLoginCommand command)
        //{
        //    var result = await _mediator.Send(command);
            
        //    if (!result.Success)
        //        return BadRequest(result);
            
        //    return Ok(result);
        //}
    }
}
