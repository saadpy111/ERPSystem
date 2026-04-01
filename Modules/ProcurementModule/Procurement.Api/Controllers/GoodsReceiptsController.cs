using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.DTOs;
using Procurement.Application.Features.GoodsReceiptFeatures.Commands;
using Procurement.Application.Features.GoodsReceiptFeatures.Queries;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

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
        [HasPermission(ProcurementPermissions.GoodsReceiptsCreate)]
        public async Task<IActionResult> Create([FromBody] CreateGoodsReceiptCommandRequest request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [HasPermission(ProcurementPermissions.GoodsReceiptsEdit)]
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
        [HasPermission(ProcurementPermissions.GoodsReceiptsDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteGoodsReceiptCommandRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("{id}/confirm")]
        [HasPermission(ProcurementPermissions.GoodsReceiptsConfirm)]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var command = new ConfirmGoodsReceiptCommandRequest { GoodsReceiptId = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [HasPermission(ProcurementPermissions.GoodsReceiptsView)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllGoodsReceiptsQueryRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(ProcurementPermissions.GoodsReceiptsView)]
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


