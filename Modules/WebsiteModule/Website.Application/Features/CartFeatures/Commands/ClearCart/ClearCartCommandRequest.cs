using MediatR;

namespace Website.Application.Features.CartFeatures.Commands.ClearCart
{
    public class ClearCartCommandRequest : IRequest<ClearCartCommandResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
