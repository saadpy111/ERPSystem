using Hr.Application.Features.SalaryStructureFeatures.Commands.CreateSalaryStructure;
using Hr.Application.Features.SalaryStructureFeatures.Commands.DeleteSalaryStructure;
using Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure;
using Hr.Application.Features.SalaryStructureFeatures.Queries.GetAllSalaryStructures;
using Hr.Application.DTOs;
using Hr.Application.Features.SalaryStructureFeatures.Queries.GetSalaryStructureById;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class SalaryStructuresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalaryStructuresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSalaryStructureRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetSalaryStructuresPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetSalaryStructureByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.SalaryStructure == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("enums/types")]
        public IActionResult GetSalaryStructureTypes()
        {
            var types = Enum.GetValues(typeof(SalaryStructureType))
                .Cast<SalaryStructureType>()
                .Select(t => new { Value = (int)t, Name = t.ToString() });
            return Ok(types);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryStructureRequest request)
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
            var request = new DeleteSalaryStructureRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("SalaryStructureMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "SalaryStructure",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "Name", Label = "Name" },
                    new OrderableFieldDto { Key = "EffectiveDate", Label = "Effective Date" },
                    new OrderableFieldDto { Key = "IsActive", Label = "Is Active" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" }
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