using MediatR;
using SharedKernel.Multitenancy;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;

namespace Website.Application.Features.NewsletterFeatures.Commands.SubscribeNewsletter
{
    public class SubscribeNewsletterCommandHandler : IRequestHandler<SubscribeNewsletterCommand, SubscribeNewsletterResponse>
    {
        private readonly INewsletterSubscriberRepository _repository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IWebsiteUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public SubscribeNewsletterCommandHandler(
            INewsletterSubscriberRepository repository,
            ITenantProvider tenantProvider,
            IWebsiteUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _repository = repository;
            _tenantProvider = tenantProvider;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<SubscribeNewsletterResponse> Handle(SubscribeNewsletterCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId()!;
            var email = request.Email.Trim().ToLowerInvariant();

            var existing = await _repository.GetByEmailAsync(email);

            if (existing != null)
            {
                if (existing.IsActive)
                    return new SubscribeNewsletterResponse { Success = true, Message = "Subscription successful" };

                existing.IsActive = true;
                existing.UnsubscribedAt = null;
                existing.UpdatedAt = System.DateTime.UtcNow;

                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new SubscribeNewsletterResponse { Success = true, Message = "Subscription successful" };
            }

            var subscriber = new NewsletterSubscriber
            {
                Email = email,
                IsActive = true,
                SubscribedAt = System.DateTime.UtcNow,
                TenantId = tenantId
            };

            await _repository.AddAsync(subscriber);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new Events.WebsiteEvents.NewsletterSubscribedEvent
            {
                Email = email,
                TenantId = tenantId
            }, cancellationToken);

            return new SubscribeNewsletterResponse { Success = true, Message = "Subscription successful" };
        }
    }
}
