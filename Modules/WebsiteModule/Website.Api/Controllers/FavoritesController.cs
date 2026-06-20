using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.FavoriteFeatures.Commands.AddFavorite;
using Website.Application.Features.FavoriteFeatures.Commands.RemoveFavorite;
using Website.Application.Features.FavoriteFeatures.Queries.GetMyFavorites;
using Website.Application.Features.FavoriteFeatures.Queries.CheckFavoriteStatus;
using Website.Application.Features.FavoriteFeatures.Queries.GetFavoritesCount;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(Guid productId)
        {
            var command = new AddFavoriteCommand
            {
                UserId = GetUserId(),
                ProductId = productId
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(Guid productId)
        {
            var command = new RemoveFavoriteCommand
            {
                UserId = GetUserId(),
                ProductId = productId
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyFavorites(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            var query = new GetMyFavoritesQuery
            {
                UserId = GetUserId(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{productId}/status")]
        public async Task<IActionResult> CheckFavoriteStatus(Guid productId)
        {
            var query = new CheckFavoriteStatusQuery
            {
                UserId = GetUserId(),
                ProductId = productId
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetFavoritesCount()
        {
            var query = new GetFavoritesCountQuery
            {
                UserId = GetUserId()
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
