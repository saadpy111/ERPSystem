using MediatR;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Enums;

namespace Subscription.Application.Features.Modules.Commands.RenewModule
{
    public class RenewModuleCommandHandler : IRequestHandler<RenewModuleCommand, RenewModuleResponse>
    {
        private readonly ITenantModuleSubscriptionRepository _tenantModuleSubscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenewModuleCommandHandler(
            ITenantModuleSubscriptionRepository tenantModuleSubscriptionRepository,
            IUnitOfWork unitOfWork)
        {
            _tenantModuleSubscriptionRepository = tenantModuleSubscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RenewModuleResponse> Handle(RenewModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscription = await _tenantModuleSubscriptionRepository.FindActiveAsync(request.TenantId, request.ModuleCode);
                if (subscription == null)
                {
                    return new RenewModuleResponse { Success = false, Error = "No active subscription found for this module" };
                }

                var now = DateTime.UtcNow;
                subscription.EndDate = subscription.Interval switch
                {
                    BillingInterval.Monthly => now.AddMonths(1),
                    BillingInterval.Yearly => now.AddYears(1),
                    BillingInterval.Quarterly => now.AddMonths(3),
                    _ => now.AddMonths(1)
                };
                subscription.UpdatedAt = now;

                await _tenantModuleSubscriptionRepository.UpdateAsync(subscription);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new RenewModuleResponse { Success = true };
            }
            catch (Exception ex)
            {
                return new RenewModuleResponse
                {
                    Success = false,
                    Error = $"Failed to renew module: {ex.Message}"
                };
            }
        }
    }
}
