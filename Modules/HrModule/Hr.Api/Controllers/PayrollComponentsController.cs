using Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent;
using Hr.Application.Features.PayrollComponentFeatures.DeletePayrollComponent;
using Hr.Application.Features.PayrollComponentFeatures.GetAllPayrollComponents;
using Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentById;
using Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentsPaged;
using Hr.Application.DTOs;
using Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class PayrollComponentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollComponentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePayrollComponentRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllPayrollComponentsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetPayrollComponentsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetPayrollComponentByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.PayrollComponent == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePayrollComponentRequest request)
        {
            request.ComponentId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeletePayrollComponentRequest { ComponentId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/types")]
        public IActionResult GetPayrollComponentTypes()
        {
            var types = Enum.GetValues(typeof(PayrollComponentType))
                .Cast<PayrollComponentType>()
                .Select(t => new { Value = (int)t, Name = t.ToString() });
            return Ok(types);
        }

        [HttpGet("PayrollComponentMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "PayrollComponent",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "Name", Label = "Name" },
                    new OrderableFieldDto { Key = "ComponentType", Label = "Component Type" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "payrollRecordId", Type = "number" },
                    new FilterableFieldDto { Key = "componentType", Type = "enum", Values = Enum.GetNames(typeof(PayrollComponentType)).ToList() }
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
