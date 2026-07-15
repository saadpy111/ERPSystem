using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Queries.GetWithdrawalRequests
{
    public class GetWithdrawalRequestsQueryHandler
        : IRequestHandler<GetWithdrawalRequestsQuery, GetWithdrawalRequestsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWithdrawalRequestsQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetWithdrawalRequestsResponse> Handle(
            GetWithdrawalRequestsQuery query,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<WithdrawalRequest>();
            var allRequests = await repo.GetAllIgnoreQueryFiltersAsync();

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
                    TenantId = r.TenantId,
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
