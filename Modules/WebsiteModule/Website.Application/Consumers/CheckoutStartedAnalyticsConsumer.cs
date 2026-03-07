using Events.WebsiteEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Consumers
{
    public class CheckoutStartedAnalyticsConsumer : INotificationHandler<CheckoutStartedEvent>
    {
        private readonly IWebsiteAnalyticsRepository _analyticsRepository;

        public CheckoutStartedAnalyticsConsumer(IWebsiteAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task Handle(CheckoutStartedEvent notification, CancellationToken cancellationToken)
        {
            await _analyticsRepository.IncrementCheckoutAsync(cancellationToken);
        }
    }
}
