using Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord;
using Hr.Application.Features.PayrollRecordFeatures.DeletePayrollRecord;
using Hr.Application.Features.PayrollRecordFeatures.GetAllPayrollRecords;
using Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordById;
using Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordsPaged;
using Hr.Application.DTOs;
using Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class PayrollRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePayrollRecordRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllPayrollRecordsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetPayrollRecordsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetPayrollRecordByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.PayrollRecord == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePayrollRecordRequest request)
        {
            request.PayrollId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeletePayrollRecordRequest { PayrollId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetPayrollStatuses()
        {
            var statuses = Enum.GetValues(typeof(PayrollStatus))
                .Cast<PayrollStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("PayrollRecordMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "PayrollRecord",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "PeriodYear", Label = "Period Year" },
                    new OrderableFieldDto { Key = "PeriodMonth", Label = "Period Month" },
                    new OrderableFieldDto { Key = "NetSalary", Label = "Net Salary" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "employeeId", Type = "number" },
                    new FilterableFieldDto { Key = "periodYear", Type = "number" },
                    new FilterableFieldDto { Key = "periodMonth", Type = "number" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(PayrollStatus)).ToList() }
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
