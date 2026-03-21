using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Features.AuthFeature.Commands.ERPLogin;
using Identity.Application.Features.AuthFeature.Commands.RegisterUser;
using Identity.Application.Features.AuthFeature.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    public class ERPAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ERPAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = new ERPLoginCommandRequest() { LoginDto = dto };

            var response = await _mediator.Send(request);

            if (!response.Success)
                return Unauthorized(new { Error = response.Error });

            return Ok(new
            {
                token = response.Token,
                user = new
                {
                    roles = response.Roles,
                    permissions = response.Permissions
                }
            });
        }

    }
}
