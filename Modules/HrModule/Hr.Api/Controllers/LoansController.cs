using Hr.Application.Features.LoanFeatures.CreateLoan;
using Hr.Application.Features.LoanFeatures.DeleteLoan;
using Hr.Application.Features.LoanFeatures.GetAllLoans;
using Hr.Application.Features.LoanFeatures.GetLoanById;
using Hr.Application.DTOs;
using Hr.Application.Features.LoanFeatures.GetLoansPaged;
using Hr.Application.Features.LoanFeatures.UpdateLoan;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLoansRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetLoansPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetLoanByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.Loan == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanRequest request)
        {
            request.Id = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteLoanRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetLoanStatuses()
        {
            var statuses = Enum.GetValues(typeof(LoanStatus))
                .Cast<LoanStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("LoanMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "Loan",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "PrincipalAmount", Label = "Principal Amount" },
                    new OrderableFieldDto { Key = "StartDate", Label = "Start Date" },
                    new OrderableFieldDto { Key = "RemainingBalance", Label = "Remaining Balance" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "employeeId", Type = "number" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(LoanStatus)).ToList() }
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
