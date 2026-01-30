using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Application.Features.ProCategoryFeatures.Commands.CreateCategory;
using Inventory.Application.Features.ProCategoryFeatures.Commands.DeleteCategory;
using Inventory.Application.Features.ProCategoryFeatures.Commands.UpdateCategory;
using Inventory.Application.Features.ProCategoryFeatures.Queries.GetAllCategories;
using Inventory.Application.Features.ProCategoryFeatures.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventories")]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        [HasPermission(InventoryPermissions.CategoriesCreate)]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto requestDto)
        {
            var request = new CreateCategoryCommandRequest() { categoryDto = requestDto };
            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("GetCategory")]
        [HasPermission(InventoryPermissions.CategoriesView)]
        public async Task<IActionResult> GetCategoryById(Guid CatId)
        {
            if (CatId == Guid.Empty)
                return BadRequest("Invalid request data.");

            GetCategoryByIdQueryRequest request = new GetCategoryByIdQueryRequest() { CategoryId = CatId };
            var response = await _mediator.Send(request);
            if(response.CategoryDto == null)
                return BadRequest("Invalid request data.");

            return Ok(response);


        }

        [HttpGet("GetAll")]
        [HasPermission(InventoryPermissions.CategoriesView)]
        public async Task<IActionResult> GetAllCategories()
        {
            GetAllCategoriesQueryRequest request = new GetAllCategoriesQueryRequest();
            var response = await _mediator.Send(request);

            return Ok(response);


        }


        [HttpPut("Update")]
        [Consumes("multipart/form-data")]
        [HasPermission(InventoryPermissions.CategoriesEdit)]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryDto requestDto)
        {
            if (requestDto == null)
                return BadRequest("Invalid request data.");

            var request = new UpdateCategoryCommandRequest() { UpdateCategoryDto = requestDto };
            var response = await _mediator.Send(request);

            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(new { message = response.Message });
        }

        [HttpDelete("{id}")]
        [HasPermission(InventoryPermissions.CategoriesDelete)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var request = new DeleteCategoryCommandRequest { Id = id };
            var response = await _mediator.Send(request);

            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(new { message = response.Message });
        }
    }
}

