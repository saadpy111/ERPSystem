using MediatR;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.RepublishCategory
{
    public class RepublishCategoryCommandRequest : IRequest<RepublishCategoryCommandResponse>
    {
        public Guid WebsiteCategoryId { get; set; }
    }
}
