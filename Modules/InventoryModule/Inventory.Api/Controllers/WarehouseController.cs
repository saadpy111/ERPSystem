using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.WarehouseDtos;
using Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse;
using Inventory.Application.Features.WarehouseFeatures.Commands.UpdateWarehouse;
using Inventory.Application.Features.WarehouseFeatures.Commands.DeleteWarehouse;
using Inventory.Application.Features.WarehouseFeatures.Queries.GetAllWarehouses;
using Inventory.Application.Features.WarehouseFeatures.Queries.GetWarehouseById;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(InventoryPermissions.WarehousesView)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllWarehousesQueryRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(InventoryPermissions.WarehousesView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetWarehouseByIdQueryRequest { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(InventoryPermissions.WarehousesCreate)]
        public async Task<IActionResult> Create([FromForm] CreateWarehouseDto dto)
        {
            var result = await _mediator.Send(new CreateWarehouseCommandRequest { Warehouse = dto });
            if (!result.Success)
                return BadRequest("Invalid request data.");

            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [HasPermission(InventoryPermissions.WarehousesEdit)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateWarehouseDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _mediator.Send(new UpdateWarehouseCommandRequest { Warehouse = dto });
            if (!result.Success)
                return BadRequest("Invalid request data.");

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        [HasPermission(InventoryPermissions.WarehousesDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteWarehouseCommandRequest { Id = id });
            if (!result.Success)
                return BadRequest("Invalid request data.");

            return Ok(result.Message);
        }
    }
}
