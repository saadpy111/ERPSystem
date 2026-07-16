using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Queries.GetMyWithdrawalRequests
{
    public class GetMyWithdrawalRequestsQueryHandler
        : IRequestHandler<GetMyWithdrawalRequestsQuery, GetMyWithdrawalRequestsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetMyWithdrawalRequestsQueryHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetMyWithdrawalRequestsResponse> Handle(
            GetMyWithdrawalRequestsQuery query,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new GetMyWithdrawalRequestsResponse { Success = false, Error = "Tenant not resolved." };
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

            return new GetMyWithdrawalRequestsResponse
            {
                Success = true,
                Requests = paged.Select(r => new MyWithdrawalRequestDto
                {
                    Id = r.Id.ToString(),
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
