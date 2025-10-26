using Identity.Application.Contracts.Services;
using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Features.AuthFeature.Commands.Register;
using Identity.Application.Features.AuthFeature.Queries.Login;
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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new user with optional role
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new RegisterCommandRequest() { RegisterDto = dto };
            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(new { Errors = response.Errors });

            return Ok(new { Message = "Registration successful" });
        }

        /// <summary>
        /// Login user and get JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = new LoginQueryRequest() { LoginDto = dto };

            var response = await _mediator.Send(request);

            if (!response.Success)
                return Unauthorized(new { Error = response.Error });

            return Ok(new { Token = response.Token });
        }
    }


}
