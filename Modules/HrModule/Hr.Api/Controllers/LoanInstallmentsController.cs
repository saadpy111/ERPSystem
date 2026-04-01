using Hr.Application.Features.LoanInstallmentFeatures.DeleteLoanInstallment;
using Hr.Application.Features.LoanInstallmentFeatures.GetAllLoanInstallments;
using Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentById;
using Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged;
using Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment;
using Hr.Application.DTOs;
using Hr.Application.Features.LoanInstallmentFeatures.PayLoanInstallment;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class LoanInstallmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanInstallmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(HrPermissions.LoanInstallmentsView)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLoanInstallmentsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        [HasPermission(HrPermissions.LoanInstallmentsView)]
        public async Task<IActionResult> GetPaged([FromQuery] GetLoanInstallmentsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(HrPermissions.LoanInstallmentsView)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetLoanInstallmentByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.LoanInstallment == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [HasPermission(HrPermissions.LoanInstallmentsEdit)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanInstallmentRequest request)
        {
            request.InstallmentId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HasPermission(HrPermissions.LoanInstallmentsDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteLoanInstallmentRequest { InstallmentId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id}/pay")]
        [HasPermission(HrPermissions.LoanInstallmentsPay)]
        public async Task<IActionResult> PayInstallment(int id, [FromBody] PayLoanInstallmentRequest request)
        {
            request.InstallmentId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);

        }
        // Enum Endpoints

        [HttpGet("enums/statuses")]
        [HasPermission(HrPermissions.LoanInstallmentsView)]
        public IActionResult GetInstallmentStatuses()
        {
            var statuses = Enum.GetValues(typeof(InstallmentStatus))
                .Cast<InstallmentStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("LoanInstallmentMetadata")]
        [HasPermission(HrPermissions.LoanInstallmentsView)]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "LoanInstallment",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "DueDate", Label = "Due Date" },
                    new OrderableFieldDto { Key = "AmountDue", Label = "Amount Due" },
                    new OrderableFieldDto { Key = "PaymentDate", Label = "Payment Date" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "loanId", Type = "number" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(InstallmentStatus)).ToList() }
                },
                Pagination = new PaginationMetadataDto
                {
                    DefaultPageSize = 10,
                    MaxPageSize = 100
                }
            };

            return Ok(metadata);
        }
    }
}
