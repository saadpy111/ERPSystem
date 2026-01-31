using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.WebsiteCategoryFeatures.Commands.PublishCategory;
using Website.Application.Features.WebsiteCategoryFeatures.Commands.UnpublishCategory;
using Website.Application.Features.WebsiteCategoryFeatures.Queries.GetAllCategories;
using Website.Application.Features.WebsiteCategoryFeatures.Queries.GetInventoryCategories;
using Website.Application.Features.WebsiteCategoryFeatures.Commands.UpdateCategoryImage;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing website categories.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/categories")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebsiteCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminCategoriesController(
            IMediator mediator,
            IWebsiteCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        #region Inventory Categories (Read-Only from Inventory)

        /// <summary>
        /// List categories from Inventory module for publishing selection.
        /// Read-only, queries Inventory directly.
        /// </summary>
        [HttpGet("inventory")]
        [HasPermission(WebsitePermissions.CategoriesPublish)]
        public async Task<IActionResult> GetInventoryCategories([FromQuery] GetInventoryCategoriesQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        #endregion

        #region Website Categories (Published)

        /// <summary>
        /// Get all published website categories.
        /// </summary>
        [HttpGet]
        [HasPermission(WebsitePermissions.CategoriesView)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null, [FromQuery] bool includeTree = false)
        {
            var response = await _mediator.Send(new GetAllCategoriesQueryRequest
            {
                IsActive = isActive,
                IncludeTree = includeTree
            });
            return Ok(response.Categories);
        }


        /// <summary>
        /// Get a category by ID.
        /// </summary>
        [HttpGet("{id}")]
        [HasPermission(WebsitePermissions.CategoriesView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryRepository.GetCategoryWithChildrenAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Publish a category.
        /// </summary>
        [HttpPost("publish")]
        [HasPermission(WebsitePermissions.CategoriesPublish)]
        public async Task<IActionResult> Publish([FromBody] PublishCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(new { CategoryId = response.CategoryId });
        }

        /// <summary>
        /// Unpublish a category (deactivate).
        /// Hides category and child categories from storefront.
        /// </summary>
        [HttpPost("{id}/unpublish")]
        [HasPermission(WebsitePermissions.CategoriesManage)]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            var response = await _mediator.Send(new UnpublishCategoryCommandRequest { WebsiteCategoryId = id });
            if (!response.Success) return NotFound(response.Message);
            return Ok(new { response.Message });
        }

        /// <summary>
        /// Update a category.
        /// </summary>
        [HttpPut("{id}")]
        [HasPermission(WebsitePermissions.CategoriesEdit)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            if (request.Name != null) category.Name = request.Name;
            if (request.Slug != null) category.Slug = request.Slug;
            category.ParentCategoryId = request.ParentCategoryId;
            if (request.DisplayOrder.HasValue) category.DisplayOrder = request.DisplayOrder.Value;
            if (request.IsActive.HasValue) category.IsActive = request.IsActive.Value;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return Ok(category);
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.CategoriesDelete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Update category image.
        /// </summary>
        [HttpPut("{id}/image")]
        [Consumes("multipart/form-data")]
        [HasPermission(WebsitePermissions.CategoriesEdit)]
        public async Task<IActionResult> UpdateImage(Guid id, [FromForm] UpdateWebsiteCategoryImageCommandRequest request)
        {
            request.CategoryId = id;
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(response);
        }

        #endregion
    }

    // Request DTO
    public record UpdateCategoryRequest(
        string? Name,
        string? Slug,
        Guid? ParentCategoryId,
        int? DisplayOrder,
        bool? IsActive
    );
}

