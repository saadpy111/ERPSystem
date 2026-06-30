using MediatR;
using SharedKernel.Events;
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
        private readonly IMediator _mediator;

        public CancelModuleCommandHandler(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IEffectiveModuleService effectiveModuleService,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _effectiveModuleService = effectiveModuleService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<CancelModuleResponse> Handle(CancelModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var modulesBefore = await _effectiveModuleService.GetEffectiveModulesAsync(request.TenantId);

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
                if (!modulesBefore.ToHashSet().SetEquals(modulesAfter))
                {
                    await _mediator.Publish(new TenantModulesChangedNotification { TenantId = request.TenantId }, cancellationToken);
                }

                return new CancelModuleResponse { Success = true };
            }
            catch (Exception ex)
            {
                return new CancelModuleResponse
                {
                    Success = false,
                    Error = $"Failed to cancel module: {ex.Message}"
                };
            }
        }
    }
}
