using MediatR;
using SharedKernel.Subscription;
using Subscription.Application.Services;

namespace Subscription.Application.Features.Permissions.Commands.SyncTenantPermissions
{
    public class SyncTenantPermissionsCommandHandler
        : IRequestHandler<SyncTenantPermissionsCommand, SyncTenantPermissionsResponse>
    {
        private readonly IEffectiveModuleService _effectiveModuleService;
        private readonly ITenantRoleProvisioningService _roleProvisioningService;

        public SyncTenantPermissionsCommandHandler(
            IEffectiveModuleService effectiveModuleService,
            ITenantRoleProvisioningService roleProvisioningService)
        {
            _effectiveModuleService = effectiveModuleService;
            _roleProvisioningService = roleProvisioningService;
        }

        public async Task<SyncTenantPermissionsResponse> Handle(
            SyncTenantPermissionsCommand request,
            CancellationToken cancellationToken)
        {
            var effectiveModules = await _effectiveModuleService
                .GetEffectiveModulesAsync(request.TenantId);

            var result = await _roleProvisioningService.ProvisionRolesAsync(
                request.TenantId,
                effectiveModules);

            return new SyncTenantPermissionsResponse
            {
                Success = result.Success,
                Error = result.Error
            };
        }
    }
}
