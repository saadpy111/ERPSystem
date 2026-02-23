using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.CouponFeatures.Commands.CreateCoupon;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing coupons.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/coupons")]
    [ApiExplorerSettings(GroupName = "Website")]
//    [Authorize]
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
    //    [HasPermission(WebsitePermissions.CouponsManage)]
        public async Task<IActionResult> Create([FromBody] CreateCouponCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(response);
        }
    }
}
