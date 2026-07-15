using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Queries.GetWithdrawalRequests
{
    public class GetWithdrawalRequestsQueryHandler
        : IRequestHandler<GetWithdrawalRequestsQuery, GetWithdrawalRequestsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetWithdrawalRequestsQueryHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetWithdrawalRequestsResponse> Handle(
            GetWithdrawalRequestsQuery query,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new GetWithdrawalRequestsResponse { Success = false, Error = "Tenant not resolved." };
            }

            var repo = _unitOfWork.Repository<WithdrawalRequest>();
            var allRequests = await repo.GetAllAsync(r => r.TenantId == tenantId);

            if (!string.IsNullOrWhiteSpace(query.StatusFilter)
                && Enum.TryParse<WithdrawalRequestStatus>(query.StatusFilter, true, out var statusFilter))
            {
                allRequests = allRequests.Where(r => r.Status == statusFilter).ToList();
            }

            var ordered = allRequests
                .OrderByDescending(r => r.RequestedAt)
                .ToList();

            var totalCount = ordered.Count;
            var paged = ordered
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new GetWithdrawalRequestsResponse
            {
                Success = true,
                Requests = paged.Select(r => new WithdrawalRequestDto
                {
                    Id = r.Id.ToString(),
                    WalletId = r.WalletId.ToString(),
                    Amount = r.Amount,
                    Status = r.Status.ToString(),
                    RequestedAt = r.RequestedAt,
                    ReviewedAt = r.ReviewedAt,
                    ReviewedBy = r.ReviewedBy,
                    Notes = r.Notes
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
