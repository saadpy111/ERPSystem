using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.DTOs;
using Procurement.Application.Features.GoodsReceiptFeatures.Commands;
using Procurement.Application.Features.GoodsReceiptFeatures.Queries;
using System;
using System.Threading.Tasks;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class GoodsReceiptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoodsReceiptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGoodsReceiptCommandRequest request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoodsReceiptCommandRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteGoodsReceiptCommandRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var command = new ConfirmGoodsReceiptCommandRequest { GoodsReceiptId = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllGoodsReceiptsQueryRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetGoodsReceiptByIdQueryRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
