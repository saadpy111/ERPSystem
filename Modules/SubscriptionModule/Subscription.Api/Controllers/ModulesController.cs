using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.Multitenancy;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("api/subscription/modules")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Authorize]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;
        private readonly IModulePurchaseService _modulePurchaseService;
        private readonly ITenantProvider _tenantProvider;

        public ModulesController(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository,
            IModulePurchaseService modulePurchaseService,
            ITenantProvider tenantProvider)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
            _modulePurchaseService = modulePurchaseService;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableModules([FromQuery] string? currency = null)
        {
            var modules = await _moduleRepository.GetAllActiveAsync();
            var result = modules.Select(m =>
            {
                var prices = _modulePriceRepository.GetByModuleIdAsync(m.Id).Result;
                var filteredPrices = string.IsNullOrEmpty(currency)
                    ? prices
                    : prices.Where(p => p.CurrencyCode.Equals(currency, StringComparison.OrdinalIgnoreCase)).ToList();

                return new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.DisplayName,
                    m.Description,
                    Prices = filteredPrices.Select(p => new
                    {
                        p.CurrencyCode,
                        p.UnitPrice,
                        Interval = p.Interval.ToString(),
                        p.EffectiveFrom,
                        p.EffectiveTo
                    })
                };
            });

            return Ok(new { Success = true, Data = result });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyPurchasedModules()
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var subscriptions = await _modulePurchaseService.GetTenantPurchasedModulesAsync(tenantId);
            var result = subscriptions.Select(s => new
            {
                s.Id,
                ModuleCode = s.Module.Code,
                ModuleName = s.Module.Name,
                s.UnitPrice,
                s.CurrencyCode,
                Interval = s.Interval.ToString(),
                Status = s.Status.ToString(),
                s.StartDate,
                s.EndDate,
                s.AutoRenew
            });

            return Ok(new { Success = true, Data = result });
        }

        [HttpPost("{moduleCode}/purchase")]
        public async Task<IActionResult> PurchaseModule(string moduleCode, [FromBody] PurchaseModuleRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            if (!Enum.TryParse<BillingInterval>(request.BillingInterval, true, out var interval))
                return BadRequest(new { Success = false, Error = "Invalid billing interval. Use Monthly, Quarterly, or Yearly." });

            var result = await _modulePurchaseService.PurchaseModuleAsync(
                tenantId, moduleCode, request.CurrencyCode, interval);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{moduleCode}/cancel")]
        public async Task<IActionResult> CancelModule(string moduleCode)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _modulePurchaseService.CancelPurchasedModuleAsync(tenantId, moduleCode);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{moduleCode}/renew")]
        public async Task<IActionResult> RenewModule(string moduleCode)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrEmpty(tenantId))
                return Unauthorized();

            var result = await _modulePurchaseService.RenewPurchasedModuleAsync(tenantId, moduleCode);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

    public class PurchaseModuleRequest
    {
        public string CurrencyCode { get; set; } = "USD";
        public string BillingInterval { get; set; } = "Monthly";
    }
}
