using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Application.Features.ProductBarcodeFeatures.Commands.CreateProductBarcode;
using Inventory.Application.Features.ProductBarcodeFeatures.Commands.UpdateProductBarcode;
using Inventory.Application.Features.ProductBarcodeFeatures.Commands.DeleteProductBarcode;
using Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetProductBarcodeById;
using Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetAllProductBarcodes;
using Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetPagedProductBarcodes;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductBarcodeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductBarcodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? barcodeValue)
        {
            var response = await _mediator.Send(new GetAllProductBarcodesQueryRequest { BarcodeValue = barcodeValue });
            return Ok(response.ProductBarcodes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetProductBarcodeByIdQueryRequest { Id = id });
            if (response.ProductBarcode == null) return NotFound();
            return Ok(response.ProductBarcode);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetPagedProductBarcodesQueryRequest
            {
                Search = search,
                Page = page,
                PageSize = pageSize
            });
            return Ok(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductBarcodeDto dto)
        {
            var response = await _mediator.Send(new CreateProductBarcodeCommandRequest { ProductBarcode = dto });
            if (!response.Success) return BadRequest();
            return Ok(response.ProductBarcode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductBarcodeDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var response = await _mediator.Send(new UpdateProductBarcodeCommandRequest { ProductBarcode = dto });
            if (!response.Success) return NotFound();
            return Ok(response.ProductBarcode);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteProductBarcodeCommandRequest { Id = id });
            if (!response.Success) return NotFound();
            return NoContent();
        }
    }
}