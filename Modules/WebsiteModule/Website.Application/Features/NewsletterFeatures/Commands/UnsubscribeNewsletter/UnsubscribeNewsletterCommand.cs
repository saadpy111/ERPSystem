using MediatR;
using System;

namespace Website.Application.Features.NewsletterFeatures.Commands.UnsubscribeNewsletter
{
    public class UnsubscribeNewsletterCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
