using Microsoft.AspNetCore.Mvc;
using MediatR;
using Website.Application.DTOs;
using Website.Application.Features.AnalyticsFeatures.Queries.GetAnalyticsDashboard;
using Website.Application.Features.AnalyticsFeatures.Queries.GetProductRevenue;
using Website.Application.Features.AnalyticsFeatures.Queries.GetCategoryRevenue;
using Website.Application.Pagination;
using Website.Application.Features.AnalyticsFeatures.Queries.GetDashboardKpis;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Website.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Website")]
    [Route("api/admin/dashboard/analytics")]
    [Authorize]
    public class AdminAnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(WebsitePermissions.AnalyticsView)]
        public async Task<ActionResult<AnalyticsDashboardDto>> GetDashboardAnalytics()
        {
            var result = await _mediator.Send(new GetAnalyticsDashboardQuery());
            return Ok(result);
        }

        [HttpGet("~/api/admin/analytics/products-revenue")]
        [HasPermission(WebsitePermissions.AnalyticsView)]
        public async Task<ActionResult<PagedResult<ProductRevenueDto>>> GetProductsRevenue(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProductRevenuePagedQuery { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("~/api/admin/analytics/categories-revenue")]
        [HasPermission(WebsitePermissions.AnalyticsView)]
        public async Task<ActionResult<PagedResult<CategoryRevenueDto>>> GetCategoriesRevenue(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetCategoryRevenuePagedQuery { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("~/api/admin/analytics/dashboard-kpis")]
        [HasPermission(WebsitePermissions.AnalyticsView)]
        public async Task<ActionResult<DashboardKpiDto>> GetDashboardKpis(
            [FromQuery] int? days = null,
            [FromQuery] DateTime? fromdate = null,
            [FromQuery] DateTime? todate = null)
        {
            var query = new GetDashboardKpisQuery {FromDate = fromdate, ToDate = todate };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
