using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Website.Application.Features.AnalyticsFeatures;
using Website.Application.DTOs;
using Website.Application.Features.AnalyticsFeatures.Queries.GetAnalyticsDashboard;

namespace Website.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Website")]

    [Route("api/admin/dashboard/analytics")]
    public class AdminAnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<AnalyticsDashboardDto>> GetDashboardAnalytics()
        {
            var result = await _mediator.Send(new GetAnalyticsDashboardQuery());
            return Ok(result);
        }
    }
}
