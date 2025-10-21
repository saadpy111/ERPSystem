using Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetAllInventoryQuarantines;
using Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetInventoryQuarantineById;
using Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetPagedInventoryQuarantines;
using Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryQuarantineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryQuarantineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? productId, [FromQuery] Guid? locationId, [FromQuery] QuarantineStatus? status)
        {
            var response = await _mediator.Send(new GetAllInventoryQuarantinesQueryRequest
            {
                ProductId = productId,
                LocationId = locationId,
                Status = status
            });
            return Ok(response.InventoryQuarantines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetInventoryQuarantineByIdQueryRequest { Id = id });
            if (response.InventoryQuarantine == null) return NotFound();
            return Ok(response.InventoryQuarantine);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetPagedInventoryQuarantinesQueryRequest
            {
                Search = search,
                Page = page,
                PageSize = pageSize
            });
            return Ok(response.Result);
        }

    }

}
