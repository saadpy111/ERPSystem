using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandRequest : IRequest<UpdateProductPriceCommandResponse>
    {
        public Guid ProductId { get; set; }
        public decimal NewPrice { get; set; }
    }
}
