using MediatR;

namespace Website.Application.Features.NewsletterFeatures.Commands.SubscribeNewsletter
{
    public class SubscribeNewsletterCommand : IRequest<SubscribeNewsletterResponse>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class SubscribeNewsletterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
