using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.CreateSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.DeleteSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.UpdateSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents;
using Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetSalaryStructureComponentById;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class SalaryStructureComponentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalaryStructureComponentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSalaryStructureComponentRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSalaryStructureComponentsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetSalaryStructureComponentByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.SalaryStructureComponent == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("by-structure/{salaryStructureId}")]
        public async Task<IActionResult> GetBySalaryStructureId(int salaryStructureId)
        {
            var query = new GetSalaryStructureComponentsBySalaryStructureIdRequest { SalaryStructureId = salaryStructureId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryStructureComponentRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteSalaryStructureComponentRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("enums/types")]
        public IActionResult GetPayrollComponentTypes()
        {
            var types = Enum.GetValues(typeof(PayrollComponentType))
                .Cast<PayrollComponentType>()
                .Select(t => new { Value = (int)t, Name = t.ToString() });
            return Ok(types);
        }
    }
}