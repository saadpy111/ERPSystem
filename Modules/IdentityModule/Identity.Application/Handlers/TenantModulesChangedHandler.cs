using Identity.Application.Services;
using MediatR;
using SharedKernel.Events;
using SharedKernel.Subscription;

namespace Identity.Application.Handlers
{
    public class TenantModulesChangedHandler : INotificationHandler<TenantModulesChangedNotification>
    {
        private readonly ISubscriptionModuleChecker _subscriptionChecker;
        private readonly ITenantRoleProvisioningService _tenantRoleProvisioningService;

        public TenantModulesChangedHandler(
            ISubscriptionModuleChecker subscriptionChecker,
            ITenantRoleProvisioningService tenantRoleProvisioningService)
        {
            _subscriptionChecker = subscriptionChecker;
            _tenantRoleProvisioningService = tenantRoleProvisioningService;
        }

        public async Task Handle(TenantModulesChangedNotification notification, CancellationToken cancellationToken)
        {
            var effectiveModules = await _subscriptionChecker.GetEnabledModulesAsync(notification.TenantId);

            await _tenantRoleProvisioningService.ProvisionRolesAsync(
                notification.TenantId,
                effectiveModules);
        }
    }
}
