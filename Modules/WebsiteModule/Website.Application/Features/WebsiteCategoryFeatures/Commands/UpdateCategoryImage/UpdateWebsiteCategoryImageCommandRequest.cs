using MediatR;
using Microsoft.AspNetCore.Http;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UpdateCategoryImage
{
    public class UpdateWebsiteCategoryImageCommandRequest : IRequest<UpdateWebsiteCategoryImageCommandResponse>
    {
        public Guid CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        public bool DeleteExisting { get; set; }
    }
}
