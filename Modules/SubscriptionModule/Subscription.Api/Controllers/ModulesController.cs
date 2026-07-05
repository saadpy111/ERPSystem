using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.Multitenancy;
using Subscription.Application.Features.ModulePayment.Commands.CreateModulePurchasePayment;
using Subscription.Application.Features.ModuleRenewalPayment.Commands.CreateModuleRenewalPayment;
using Subscription.Application.Features.Modules.Commands.CancelModule;
using Subscription.Application.Features.Modules.Commands.PurchaseModule;
using Subscription.Application.Features.Modules.Commands.RenewModule;
using Subscription.Application.Features.Modules.Queries.GetAvailableModules;
using Subscription.Application.Features.Modules.Queries.GetEffectiveModules;
using Subscription.Application.Features.Modules.Queries.GetPurchasedModules;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscription/modules")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Authorize]
    public class ModulesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public ModulesController(IMediator mediator, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableModules([FromQuery] string? currency = null)
        {
            var result = await _mediator.Send(new GetAvailableModulesQuery
            {
                CurrencyCode = currency
            });

            return Ok(result);
        }

        [HttpGet("purchased")]
        public async Task<IActionResult> GetMyPurchasedModules()
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _mediator.Send(new GetPurchasedModulesQuery
            {
                TenantId = tenantId
            });

            return Ok(result);
        }

        [HttpGet("effective")]
        public async Task<IActionResult> GetEffectiveModules()
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _mediator.Send(new GetEffectiveModulesQuery
            {
                TenantId = tenantId
            });

            return Ok(result);
        }

        //[HttpPost("{moduleCode}/purchase")]
        //public async Task<IActionResult> PurchaseModule(string moduleCode, [FromBody] PurchaseModuleRequest request)
        //{
        //    var tenantId = _tenantProvider.GetTenantId();
        //    if (string.IsNullOrEmpty(tenantId))
        //        return Unauthorized();

        //    if (!Enum.TryParse<BillingInterval>(request.BillingInterval, true, out var interval))
        //        return BadRequest(new { Success = false, Error = "Invalid billing interval. Use Monthly, Quarterly, or Yearly." });

        //    var result = await _mediator.Send(new PurchaseModuleCommand
        //    {
        //        TenantId = tenantId,
        //        ModuleCode = moduleCode,
        //        CurrencyCode = request.CurrencyCode,
        //        Interval = interval
        //    });

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpPost("{moduleCode}/cancel")]
        public async Task<IActionResult> CancelModule(string moduleCode)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _mediator.Send(new CancelModuleCommand
            {
                TenantId = tenantId,
                ModuleCode = moduleCode
            });

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        //[HttpPost("{moduleCode}/renew")]
        //public async Task<IActionResult> RenewModule(string moduleCode)
        //{
        //    var tenantId = _tenantProvider.GetTenantId();
        //    if (string.IsNullOrEmpty(tenantId))
        //        return Unauthorized();

        //    var result = await _mediator.Send(new RenewModuleCommand
        //    {
        //        TenantId = tenantId,
        //        ModuleCode = moduleCode
        //    });

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpPost("payment")]
        public async Task<IActionResult> PurchaseModulePayment([FromBody] ModulePurchasePaymentRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!Enum.TryParse<BillingInterval>(request.BillingInterval, true, out var interval))
                return BadRequest(new { Success = false, Error = "Invalid billing interval. Use Monthly, Quarterly, or Yearly." });

            var result = await _mediator.Send(new CreateModulePurchasePaymentCommand
            {
                TenantId = tenantId,
                UserId = userId,
                ModuleCode = request.ModuleCode,
                CurrencyCode = request.CurrencyCode,
                Interval = interval,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            });

            if (!result.Success)
                return BadRequest(new { Success = false, Error = result.Error });

            return Ok(result);
        }

        [HttpPost("renew/payment")]
        public async Task<IActionResult> RenewModulePayment([FromBody] ModuleRenewalPaymentRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _mediator.Send(new CreateModuleRenewalPaymentCommand
            {
                TenantId = tenantId,
                UserId = userId,
                ModuleCode = request.ModuleCode,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            });

            if (!result.Success)
                return BadRequest(new { Success = false, Error = result.Error });

            return Ok(result);
        }
    }

    public class PurchaseModuleRequest
    {
        public string CurrencyCode { get; set; } = "USD";
        public string BillingInterval { get; set; } = "Monthly";
    }

    public sealed class ModulePurchasePaymentRequest
    {
        public string ModuleCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "USD";
        public string BillingInterval { get; set; } = "Monthly";
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed class ModuleRenewalPaymentRequest
    {
        public string ModuleCode { get; set; } = string.Empty;
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
