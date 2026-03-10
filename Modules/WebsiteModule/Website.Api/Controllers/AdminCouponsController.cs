using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.CouponFeatures.Commands.CreateCoupon;
using Website.Application.Features.CouponFeatures.Queries.GetCouponById;
using Website.Application.Features.CouponFeatures.Queries.GetCouponsPaged;
using Website.Application.Pagination;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing coupons.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/coupons")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminCouponsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminCouponsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new coupon.
        /// </summary>
        [HttpPost]
     //   [HasPermission(WebsitePermissions.CouponsManage)]
        public async Task<IActionResult> Create([FromBody] CreateCouponCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(response);
        }

        /// <summary>
        /// Get paginated coupons list.
        /// </summary>
        [HttpGet]
      //   [HasPermission(WebsitePermissions.CouponsView)]
        public async Task<IActionResult> GetAll([FromQuery] CouponFilter filter)
        {
            var response = await _mediator.Send(new GetCouponsPagedQueryRequest { Filter = filter });
            return Ok(response);
        }

        /// <summary>
        /// Get coupon details by ID.
        /// </summary>
        [HttpGet("{id}")]
     //   [HasPermission(WebsitePermissions.CouponsView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetCouponByIdQueryRequest { Id = id });
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
