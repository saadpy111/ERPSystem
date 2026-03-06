using Events.IdentityEvents;
using MediatR;
using Website.Application.Contracts.Persistence;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Consumers
{
    /// <summary>
    /// Handles the ClientRegisteredEvent by creating a CustomerProfile
    /// in the WebsiteModule database.
    /// 
    /// Published by: IdentityModule when a client completes registration.
    /// Consumed by: WebsiteModule to maintain customer profile.
    /// </summary>
    public class ClientRegisteredConsumer : INotificationHandler<ClientRegisteredEvent>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IWebsiteUnitOfWork _websiteUnitOfWork;

        public ClientRegisteredConsumer(ICustomerRepository customerRepository , IWebsiteUnitOfWork websiteUnitOfWork)
        {
            _customerRepository = customerRepository;
            _websiteUnitOfWork = websiteUnitOfWork;
        }

        public async Task Handle(ClientRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var profile = new CustomerProfile
            {
                UserId = notification.UserId,
                TenantId = notification.TenantId,
                CustomerSince = DateTime.UtcNow,
                AllowMarketingEmails = true
            };

            await _customerRepository.CreateAsync(profile, cancellationToken);
        }
    }
}
