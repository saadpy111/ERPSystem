using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.CreateProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.UpdateProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.DeleteProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Queries.GetAllProductAttributes;
using Inventory.Application.Features.ProductAttributeFeatures.Queries.GetProductAttributeById;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]
    [Authorize]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductAttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(InventoryPermissions.AttributesView)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductAttributesQueryRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(InventoryPermissions.AttributesView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductAttributeByIdQueryRequest { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(InventoryPermissions.AttributesCreate)]
        public async Task<IActionResult> Create([FromBody] CreateProductAttributeDto dto)
        {
            var result = await _mediator.Send(new CreateProductAttributeCommandRequest { ProductAttribute = dto });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [HasPermission(InventoryPermissions.AttributesEdit)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductAttributeDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _mediator.Send(new UpdateProductAttributeCommandRequest { ProductAttribute = dto });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HasPermission(InventoryPermissions.AttributesDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductAttributeCommandRequest { Id = id });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
