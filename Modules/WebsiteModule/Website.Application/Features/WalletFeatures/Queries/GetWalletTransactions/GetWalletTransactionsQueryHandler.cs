using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WalletFeatures.Queries.GetWalletTransactions
{
    public class GetWalletTransactionsQueryHandler
        : IRequestHandler<GetWalletTransactionsQuery, GetWalletTransactionsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetWalletTransactionsQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetWalletTransactionsResponse> Handle(
            GetWalletTransactionsQuery query,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new GetWalletTransactionsResponse { Success = false, Error = "Tenant not resolved." };
            }

            var wallets = await _unitOfWork.Repository<Wallet>()
                .GetAllAsync(w => w.TenantId == tenantId);
            var wallet = wallets.FirstOrDefault();

            if (wallet == null)
            {
                return new GetWalletTransactionsResponse
                {
                    Success = true,
                    Transactions = new List<WalletTransactionDto>(),
                    TotalCount = 0,
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize
                };
            }

            var allTxs = await _unitOfWork.Repository<WalletTransaction>()
                .GetAllAsync(t => t.WalletId == wallet.Id);

            var orderedTxs = allTxs
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            var totalCount = orderedTxs.Count;
            var pagedTxs = orderedTxs
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new GetWalletTransactionsResponse
            {
                Success = true,
                Transactions = pagedTxs.Select(t => new WalletTransactionDto
                {
                    Id = t.Id.ToString(),
                    Type = t.Type.ToString(),
                    Amount = t.Amount,
                    BalanceBefore = t.BalanceBefore,
                    BalanceAfter = t.BalanceAfter,
                    Reference = t.Reference,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
