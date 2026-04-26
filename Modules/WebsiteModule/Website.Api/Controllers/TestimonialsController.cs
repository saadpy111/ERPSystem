using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;
using System;
using System.Threading.Tasks;
using Website.Application.Features.Testimonials.Commands.CreateTestimonial;
using Website.Application.Features.Testimonials.Commands.DeleteTestimonial;
using Website.Application.Features.Testimonials.Commands.ReorderTestimonials;
using Website.Application.Features.Testimonials.Commands.UpdateTestimonial;
using Website.Application.Features.Testimonials.Commands.UpdateVisibility;
using Website.Application.Features.Testimonials.Queries.GetAllTestimonials;
using Website.Application.Features.Testimonials.Queries.GetTestimonialById;
using Website.Application.Features.Testimonials.Queries.GetVisibleTestimonials;

namespace Website.Api.Controllers
{
    [Route("api/website/[controller]")]
    [ApiExplorerSettings(GroupName = "Website")]

    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestimonialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicTestimonials([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetVisibleTestimonialsQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        [HttpGet]
        [HasPermission(WebsitePermissions.TestimonialsView)]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllTestimonialsQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(WebsitePermissions.TestimonialsView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTestimonialByIdQuery { Id = id });
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestimonialCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [HasPermission(WebsitePermissions.TestimonialsEdit)]
        public async Task<IActionResult> Update([FromBody] UpdateTestimonialCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.TestimonialsDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTestimonialCommand { Id = id });
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}/visibility")]
        [HasPermission(WebsitePermissions.TestimonialsChangeVisibility)]
        public async Task<IActionResult> UpdateVisibility(Guid id)
        {
            var result = await _mediator.Send(new UpdateVisibilityCommand { Id = id });
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("reorder")]
        [HasPermission(WebsitePermissions.TestimonialsReorder)]
        public async Task<IActionResult> Reorder([FromBody] ReorderTestimonialsCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
