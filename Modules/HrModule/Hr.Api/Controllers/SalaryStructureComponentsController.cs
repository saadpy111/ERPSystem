using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.CreateSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.DeleteSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.UpdateSalaryStructureComponent;
using Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents;
using Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetSalaryStructureComponentById;
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
    public class SalaryStructureComponentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalaryStructureComponentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HasPermission(HrPermissions.SalaryComponentsCreate)]
        public async Task<IActionResult> Create([FromBody] CreateSalaryStructureComponentRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [HasPermission(HrPermissions.SalaryComponentsView)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSalaryStructureComponentsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(HrPermissions.SalaryComponentsView)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetSalaryStructureComponentByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.SalaryStructureComponent == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("by-structure/{salaryStructureId}")]
        [HasPermission(HrPermissions.SalaryComponentsView)]
        public async Task<IActionResult> GetBySalaryStructureId(int salaryStructureId)
        {
            var query = new GetSalaryStructureComponentsBySalaryStructureIdRequest { SalaryStructureId = salaryStructureId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [HasPermission(HrPermissions.SalaryComponentsEdit)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryStructureComponentRequest request)
        {
            request.Id = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HasPermission(HrPermissions.SalaryComponentsDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteSalaryStructureComponentRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("enums/types")]
        [HasPermission(HrPermissions.SalaryComponentsView)]
        public IActionResult GetPayrollComponentTypes()
        {
            var types = Enum.GetValues(typeof(PayrollComponentType))
                .Cast<PayrollComponentType>()
                .Select(t => new { Value = (int)t, Name = t.ToString() });
            return Ok(types);
        }
    }
}
