using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.LocationDtos;
using Inventory.Application.Features.LocationFeatures.Commands.CreateLocation;
using Inventory.Application.Features.LocationFeatures.Commands.UpdateLocation;
using Inventory.Application.Features.LocationFeatures.Commands.DeleteLocation;
using Inventory.Application.Features.LocationFeatures.Queries.GetLocationById;
using Inventory.Application.Features.LocationFeatures.Queries.GetAllLocations;
using Inventory.Application.Features.LocationFeatures.Queries.GetLocationsByWarehouseId;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]


    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? warehouseId)
        {
            var response = await _mediator.Send(new GetAllLocationsQueryRequest { WarehouseId = warehouseId });
            return Ok(response.Locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetLocationByIdQueryRequest { Id = id });
            if (response.Location == null) return NotFound();
            return Ok(response.Location);
        }

        [HttpGet("by-warehouse/{warehouseId}")]
        public async Task<IActionResult> GetByWarehouseId(Guid warehouseId)
        {
            var response = await _mediator.Send(new GetLocationsByWarehouseIdQueryRequest { WarehouseId = warehouseId });
            return Ok(response.Locations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationDto dto)
        {
            var response = await _mediator.Send(new CreateLocationCommandRequest { Location = dto });
            if (!response.Success) return BadRequest();
            return Ok(response.Location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLocationDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var response = await _mediator.Send(new UpdateLocationCommandRequest { Location = dto });
            if (!response.Success) return NotFound();
            return Ok(response.Location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteLocationCommandRequest { Id = id });
            if (!response.Success) return NotFound();
            return NoContent();
        }
    }
}