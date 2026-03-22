using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Website.Application.DTOs;
using Website.Application.Features.DashboardFeatures.Queries.GetDashboardOverview;
using Website.Application.Features.DashboardFeatures.Queries.GetWeeklyOverview;
using System.Collections.Generic;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Website.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Website")]
    [Route("api/admin/dashboard/overview")]
    [Authorize]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(WebsitePermissions.DashboardView)]
        public async Task<ActionResult<DashboardOverviewDto>> GetDashboardOverview(
            [FromQuery] int? days = null,
            [FromQuery] DateTime? fromdate = null,
            [FromQuery] DateTime? todate = null)
        {
            var query = new GetDashboardOverviewQuery { Days = days, FromDate = fromdate, ToDate = todate };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("~/api/admin/analytics/weekly-overview")]
        [HasPermission(WebsitePermissions.DashboardView)]
        public async Task<ActionResult<List<WeeklyAnalyticsDto>>> GetWeeklyOverview([FromQuery] int? days = 7)
        {
            var query = new GetWeeklyOverviewQuery { Days = days };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
