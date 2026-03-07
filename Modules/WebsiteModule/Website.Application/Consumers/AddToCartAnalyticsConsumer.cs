using Events.WebsiteEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Consumers
{
    public class AddToCartAnalyticsConsumer : INotificationHandler<AddToCartEvent>
    {
        private readonly IWebsiteAnalyticsRepository _analyticsRepository;

        public AddToCartAnalyticsConsumer(IWebsiteAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task Handle(AddToCartEvent notification, CancellationToken cancellationToken)
        {
            await _analyticsRepository.IncrementAddToCartAsync(cancellationToken);
        }
    }
}
