using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistories;
using Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistoryById;
using Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetPagedProductCostHistories;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "inventories")]
    [Authorize]
    public class ProductCostHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCostHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(InventoryPermissions.ProductsViewCostHistory)]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? productId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var response = await _mediator.Send(new GetProductCostHistoriesQueryRequest
            {
                ProductId = productId,
                FromDate = fromDate,
                ToDate = toDate
            });
            return Ok(response.CostHistories);
        }

        [HttpGet("{id}")]
        [HasPermission(InventoryPermissions.ProductsViewCostHistory)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetProductCostHistoryByIdQueryRequest { Id = id });
            if (response.ProductCostHistory == null) return NotFound();
            return Ok(response.ProductCostHistory);
        }

        [HttpGet("paged")]
        [HasPermission(InventoryPermissions.ProductsViewCostHistory)]
        public async Task<IActionResult> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetPagedProductCostHistoriesQueryRequest
            {
                Search = search,
                Page = page,
                PageSize = pageSize
            });
            return Ok(response.Result);
        }
    }
}
