using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.Themes.Commands.CreateTheme;
using Website.Application.Features.Themes.Commands.DeleteTheme;
using Website.Application.Features.Themes.Commands.UpdateTheme;
using Website.Application.Features.Themes.Queries.GetAllThemes;
using Website.Application.Features.Themes.Queries.GetThemeById;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Theme management API (Admin endpoints).
    /// </summary>
    [ApiController]
    [Route("api/website/themes")]
    [ApiExplorerSettings(GroupName = "Website")]
    public class ThemesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ThemesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all themes (PUBLIC - for theme selection)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAllThemesResponse), 200)]
        public async Task<IActionResult> GetAllThemes([FromQuery] bool activeOnly = true)
        {
            var result = await _mediator.Send(new GetAllThemesQuery { ActiveOnly = activeOnly });
            return Ok(result);
        }

        /// <summary>
        /// Get theme by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetThemeByIdResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetThemeById(Guid id)
        {
            var result = await _mediator.Send(new GetThemeByIdQuery { ThemeId = id });
            
            if (!result.Success)
                return NotFound(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Create new theme (ADMIN ONLY)
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "PlatformAdmin")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CreateThemeResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTheme([FromForm] CreateThemeCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            
            return CreatedAtAction(nameof(GetThemeById), new { id = result.ThemeId }, result);
        }

        /// <summary>
        /// Update theme (ADMIN ONLY)
        /// </summary>
        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(UpdateThemeResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTheme(Guid id, [FromForm] UpdateThemeCommand command)
        {
            command.ThemeId = id;
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return NotFound(new { error = result.Error });
            
            return Ok(result);
        }

        /// <summary>
        /// Delete theme (ADMIN ONLY)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTheme(Guid id)
        {
            var result = await _mediator.Send(new DeleteThemeCommand { ThemeId = id });
            
            if (!result.Success)
                return NotFound(new { error = result.Error });
            
            return Ok(result);
        }
    }
}
