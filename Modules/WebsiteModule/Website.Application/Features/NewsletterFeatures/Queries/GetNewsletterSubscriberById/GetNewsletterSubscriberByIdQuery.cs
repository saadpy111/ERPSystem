using MediatR;
using System;
using Website.Application.DTOs;

namespace Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscriberById
{
    public class GetNewsletterSubscriberByIdQuery : IRequest<NewsletterSubscriberDto?>
    {
        public Guid Id { get; set; }
    }
}
