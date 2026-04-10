using Accounting.Application.Features.Budgets.Commands.ChangeBudgetStatus;
using Accounting.Application.Features.Budgets.Commands.CreateBudget;
using Accounting.Application.Features.Budgets.Commands.UpdateBudget;
using Accounting.Application.Features.Budgets.DTOs;
using Accounting.Application.Features.Budgets.Queries.GetAllBudgets;
using Accounting.Application.Features.Budgets.Queries.GetBudgetById;
using Accounting.Application.Features.Budgets.Queries.GetBudgetVsActual;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/budgets")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class BudgetsController : ControllerBase

    {
        private readonly IMediator _mediator;

        public BudgetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.BudgetsCreate)]
        public async Task<IActionResult> Create(CreateBudgetCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [HasPermission(AccountingPermissions.BudgetsEdit)]
        public async Task<IActionResult> Update(int id, UpdateBudgetCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/status")]
        [HasPermission(AccountingPermissions.BudgetsEdit)]
        public async Task<IActionResult> ChangeStatus(int id, ChangeBudgetStatusCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [HasPermission(AccountingPermissions.BudgetsView)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBudgetByIdQuery { Id = id });
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.BudgetsView)]
        public async Task<IActionResult> GetAll([FromQuery] int? fiscalYearId)
        {
            var result = await _mediator.Send(new GetAllBudgetsQuery { FiscalYearId = fiscalYearId });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/report/vs-actual")]
        [HasPermission(AccountingPermissions.BudgetsReport)]
        public async Task<IActionResult> GetVsActualReport(int id)
        {
            var result = await _mediator.Send(new GetBudgetVsActualQuery { BudgetId = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
