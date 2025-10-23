using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.StockQuantDtos;

using Inventory.Application.Features.StockQuantFeatures.Queries.GetStockQuantById;
using Inventory.Application.Features.StockQuantFeatures.Queries.GetAllStockQuants;
using Inventory.Application.Features.StockQuantFeatures.Queries.GetPagedStockQuants;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]

    public class StockQuantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockQuantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? productId, [FromQuery] Guid? locationId)
        {
            var response = await _mediator.Send(new GetAllStockQuantsQueryRequest { ProductId = productId, LocationId = locationId });
            return Ok(response.StockQuants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetStockQuantByIdQueryRequest { Id = id });
            if (response.StockQuant == null) return NotFound();
            return Ok(response.StockQuant);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetPagedStockQuantsQueryRequest
            {
                Search = search,
                Page = page,
                PageSize = pageSize
            });
            return Ok(response.Result);
        }
    }
}