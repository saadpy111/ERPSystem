using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Application.DTOs;

namespace Website.Application.Features.NewsletterFeatures.Queries.GetNewsletterSubscriberById
{
    public class GetNewsletterSubscriberByIdQueryHandler : IRequestHandler<GetNewsletterSubscriberByIdQuery, NewsletterSubscriberDto?>
    {
        private readonly INewsletterSubscriberRepository _repository;

        public GetNewsletterSubscriberByIdQueryHandler(INewsletterSubscriberRepository repository)
        {
            _repository = repository;
        }

        public async Task<NewsletterSubscriberDto?> Handle(GetNewsletterSubscriberByIdQuery request, CancellationToken cancellationToken)
        {
            var subscriber = await _repository.GetByIdAsync(request.Id);
            if (subscriber == null)
                return null;

            return new NewsletterSubscriberDto
            {
                Id = subscriber.Id,
                Email = subscriber.Email,
                IsActive = subscriber.IsActive,
                SubscribedAt = subscriber.SubscribedAt
            };
        }
    }
}
