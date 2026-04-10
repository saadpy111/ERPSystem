using Accounting.Application.Features.Vouchers.Commands.ApproveVoucher;
using Accounting.Application.Features.Vouchers.Commands.CreateVoucher;
using Accounting.Application.Features.Vouchers.Commands.PostVoucher;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/vouchers")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class VouchersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VouchersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.VouchersCreate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost("{id:int}/approve")]
        [HasPermission(AccountingPermissions.VouchersApprove)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Approve([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new ApproveVoucherCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost("{id:int}/post")]
        [HasPermission(AccountingPermissions.VouchersPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new PostVoucherCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
