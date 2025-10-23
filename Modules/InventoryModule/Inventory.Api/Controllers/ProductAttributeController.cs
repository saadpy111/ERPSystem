using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.ProductAttributeDtos;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.CreateProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.UpdateProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Commands.DeleteProductAttribute;
using Inventory.Application.Features.ProductAttributeFeatures.Queries.GetAllProductAttributes;
using Inventory.Application.Features.ProductAttributeFeatures.Queries.GetProductAttributeById;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]

    public class ProductAttributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductAttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductAttributesQueryRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductAttributeByIdQueryRequest { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductAttributeDto dto)
        {
            var result = await _mediator.Send(new CreateProductAttributeCommandRequest { ProductAttribute = dto });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductAttributeDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _mediator.Send(new UpdateProductAttributeCommandRequest { ProductAttribute = dto });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductAttributeCommandRequest { Id = id });
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}