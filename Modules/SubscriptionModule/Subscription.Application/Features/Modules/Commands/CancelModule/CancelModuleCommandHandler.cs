using MediatR;
using SharedKernel.Subscription;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Services;
using Subscription.Domain.Enums;

namespace Subscription.Application.Features.Modules.Commands.CancelModule
{
    public class CancelModuleCommandHandler : IRequestHandler<CancelModuleCommand, CancelModuleResponse>
    {
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IEffectiveModuleService _effectiveModuleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantRoleProvisioningService _tenantRoleProvisioningService;

        public CancelModuleCommandHandler(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService,
            IUnitOfWork unitOfWork,
            ITenantRoleProvisioningService tenantRoleProvisioningService)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
            _unitOfWork = unitOfWork;
            _tenantRoleProvisioningService = tenantRoleProvisioningService;
        }

        public async Task<CancelModuleResponse> Handle(CancelModuleCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var subscription = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, request.ModuleCode);
                if (subscription == null)
                {
                    return new CancelModuleResponse { Success = false, Error = "No active subscription found for this module" };
                }

                subscription.Status = ModuleSubscriptionStatus.Cancelled;
                subscription.EndDate = DateTime.UtcNow;
                subscription.UpdatedAt = DateTime.UtcNow;

                await _tenantModuleSubscriptionRepository.UpdateAsync(subscription);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var modulesAfter = await _effectiveModuleService.GetEffectiveModulesAsync(request.TenantId);

                var provisionResult = await _tenantRoleProvisioningService.ProvisionRolesAsync(
                    request.TenantId,
                    modulesAfter);

                if (!provisionResult.Success)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new CancelModuleResponse
                    {
                        Success = false,
                        Error = provisionResult.Error
                    };
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new CancelModuleResponse { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CancelModuleResponse
                {
                    Success = false,
                    Error = $"Failed to cancel module: {ex.Message}"
                };
            }
        }
    }
}
