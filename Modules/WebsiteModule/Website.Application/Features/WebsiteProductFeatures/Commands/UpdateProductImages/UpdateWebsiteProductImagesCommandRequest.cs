using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductImages
{
    public class UpdateWebsiteProductImagesCommandRequest : IRequest<UpdateWebsiteProductImagesCommandResponse>
    {
        public Guid ProductId { get; set; }
        public List<WebsiteProductImageUpdateDto> Images { get; set; } = new();
    }
}
