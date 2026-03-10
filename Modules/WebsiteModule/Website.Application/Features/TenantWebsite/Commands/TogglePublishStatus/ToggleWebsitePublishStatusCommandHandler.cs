using MediatR;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.TenantWebsite.Commands.TogglePublishStatus
{
    public class ToggleWebsitePublishStatusCommandHandler : IRequestHandler<ToggleWebsitePublishStatusCommand, ToggleWebsitePublishStatusResponse>
    {
        private readonly ITenantWebsiteRepository _tenantWebsiteRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public ToggleWebsitePublishStatusCommandHandler(ITenantWebsiteRepository tenantWebsiteRepository, IWebsiteUnitOfWork unitOfWork)
        {
            _tenantWebsiteRepository = tenantWebsiteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ToggleWebsitePublishStatusResponse> Handle(ToggleWebsitePublishStatusCommand request, CancellationToken cancellationToken)
        {
            var tenantWebsite = await _tenantWebsiteRepository.GetByTenantIdAsync(request.TenantId);

            if (tenantWebsite == null)
            {
                return new ToggleWebsitePublishStatusResponse
                {
                    Success = false,
                    Error = "Tenant website configuration not found"
                };
            }

            // Toggle publish status
            tenantWebsite.IsPublished = !tenantWebsite.IsPublished;
            tenantWebsite.UpdatedAt = DateTime.UtcNow;

            await _tenantWebsiteRepository.UpdateAsync(tenantWebsite);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ToggleWebsitePublishStatusResponse
            {
                Success = true,
                NewPublishStatus = tenantWebsite.IsPublished
            };
        }
    }
}
