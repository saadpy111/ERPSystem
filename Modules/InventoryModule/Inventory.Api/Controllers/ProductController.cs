using Inventory.Application.Dtos.ProductDtos;
using Inventory.Application.Features.ProductFeatures.Commands.AddProductAttributeValues;
using Inventory.Application.Features.ProductFeatures.Commands.CreateProduct;
using Inventory.Application.Features.ProductFeatures.Commands.DeleteProduct;
using Inventory.Application.Features.ProductFeatures.Commands.MakeProductInactive;
using Inventory.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Inventory.Application.Features.ProductFeatures.Queries.GetPagedProducts;
using Inventory.Application.Features.ProductFeatures.Queries.GetProductById;
using Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        [HasPermission(InventoryPermissions.ProductsCreate)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            var response = await _mediator.Send(new CreateProductCommandRequest { Product = dto });
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("{id}/attribute-values")]
        [HasPermission(InventoryPermissions.ProductsManageAttributes)]
        public async Task<IActionResult> AddAttributeValues(Guid id, [FromBody] List<ProductAttributeValueDto> values)
        {
            var response = await _mediator.Send(new AddProductAttributeValuesCommandRequest
            {
                ProductId = id,
                AttributeValues = values
            });
            if (!response.Success) return BadRequest();
            return NoContent();
        }

        [HttpGet("{id}")]
        [HasPermission(InventoryPermissions.ProductsView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetProductByIdQueryRequest { Id = id });
            if (response.Product == null) return NotFound();
            return Ok(response.Product);
        }

        [HttpDelete("{id}")]
        [HasPermission(InventoryPermissions.ProductsDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteProductCommandRequest { Id = id });
            if (!response.Success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/inactive")]
        [HasPermission(InventoryPermissions.ProductsDeactivate)]
        public async Task<IActionResult> MakeInactive(Guid id)
        {
            var response = await _mediator.Send(new MakeProductInactiveCommandRequest { Id = id });
            if (!response.Success) return NotFound();
            return NoContent();
        }


        [HttpGet("paged")]
        [HasPermission(InventoryPermissions.ProductsView)]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetPagedProductsQueryRequest
            {
                Search = search,
                Page = page,
                PageSize = pageSize
            });
            return Ok(response.Result);
        }

        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        [HasPermission(InventoryPermissions.ProductsEdit)]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto dto)
        {
            var response = await _mediator.Send(new UpdateProductCommandRequest { Product = dto });
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("GetProductImageTypes")]
        [HasPermission(InventoryPermissions.ProductsView)]
        public IActionResult GetProductImageTypes()
        {
            var values = Enum.GetValues(typeof(ProductImageType))
                .Cast<ProductImageType>()
                .Select(t => new
                {
                    Id = (int)t,
                    Name = t.ToString()
                });

            return Ok(values);
        }

    }
}
