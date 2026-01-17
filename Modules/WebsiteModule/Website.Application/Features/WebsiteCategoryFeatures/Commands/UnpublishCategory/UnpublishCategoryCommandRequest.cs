using MediatR;

namespace Website.Application.Features.WebsiteCategoryFeatures.Commands.UnpublishCategory
{
    public class UnpublishCategoryCommandRequest : IRequest<UnpublishCategoryCommandResponse>
    {
        public Guid WebsiteCategoryId { get; set; }
    }
}
