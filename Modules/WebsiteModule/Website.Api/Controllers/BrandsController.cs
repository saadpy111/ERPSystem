using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Website.Application.Features.Brands.Commands.CreateBrand;
using Website.Application.Features.Brands.Commands.DeleteBrand;
using Website.Application.Features.Brands.Commands.UpdateBrand;
using Website.Application.Features.Brands.Queries.GetAllBrands;
using Website.Application.Features.Brands.Queries.GetBrandById;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/website/brands")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBrandsQuery());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetBrandByIdQuery { Id = id });
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [HasPermission(WebsitePermissions.BrandsCreate)]
        public async Task<IActionResult> Create([FromForm] CreateBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok( response);
        }

        [HttpPut]
        [HasPermission(WebsitePermissions.BrandsEdit)]
        public async Task<IActionResult> Update([FromForm] UpdateBrandCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.BrandsDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response= await _mediator.Send(new DeleteBrandCommand { Id = id });
            if (!response.Success) return NotFound();
            return NoContent();
        }
    }
}
