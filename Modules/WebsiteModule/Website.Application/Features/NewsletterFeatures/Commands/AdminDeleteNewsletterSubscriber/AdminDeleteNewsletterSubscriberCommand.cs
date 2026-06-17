using MediatR;
using System;

namespace Website.Application.Features.NewsletterFeatures.Commands.AdminDeleteNewsletterSubscriber
{
    public class AdminDeleteNewsletterSubscriberCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
