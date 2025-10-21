using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Features.ProCategoryFeatures.Commands.CreateCategory;
using Inventory.Application.Features.ProCategoryFeatures.Commands.DeleteCategory;
using Inventory.Application.Features.ProCategoryFeatures.Commands.UpdateCategory;
using Inventory.Application.Features.ProCategoryFeatures.Queries.GetAllCategories;
using Inventory.Application.Features.ProCategoryFeatures.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategoryById(Guid CatId)
        {
            if(CatId == null)
                return BadRequest("Invalid request data.");

            GetCategoryByIdQueryRequest request = new GetCategoryByIdQueryRequest() { CategoryId = CatId };
            var response = await _mediator.Send(request);

            return Ok(response);


        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            GetAllCategoriesQueryRequest request = new  GetAllCategoriesQueryRequest();
            var response = await _mediator.Send(request);

            return Ok(response);


        }


        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommandRequest request)
        {
            if (request == null || request.UpdateCategoryDto == null)
                return BadRequest("Invalid request data.");

            var response = await _mediator.Send(request);

            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(new { message = response.Message });
        }

        [HttpDelete("{id}")]
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
