using Identity.Application.Contracts.Services;
using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Features.AuthFeature.Commands.RegisterUser;
using Identity.Application.Features.AuthFeature.Queries.Login;
using Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class WebAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = new LoginQueryRequest() { LoginDto = dto };

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


        [HttpPost("register-client")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(new { error = response.Error });

            return Ok(new { message = response.Message, userId = response.UserId });
        }
    





    }
}
