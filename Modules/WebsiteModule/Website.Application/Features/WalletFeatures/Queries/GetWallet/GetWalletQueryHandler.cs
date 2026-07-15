using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.WalletFeatures.Queries.GetWallet
{
    public class GetWalletQueryHandler
        : IRequestHandler<GetWalletQuery, GetWalletResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetWalletQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetWalletResponse> Handle(
            GetWalletQuery query,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new GetWalletResponse { Success = false, Error = "Tenant not resolved." };
            }

            var wallets = await _unitOfWork.Repository<Wallet>()
                .GetAllAsync(w => w.TenantId == tenantId);
            var wallet = wallets.FirstOrDefault();

            if (wallet == null)
            {
                return new GetWalletResponse
                {
                    Success = true,
                    WalletId = null,
                    CurrentBalance = 0
                };
            }

            return new GetWalletResponse
            {
                Success = true,
                WalletId = wallet.Id.ToString(),
                CurrentBalance = wallet.CurrentBalance
            };
        }
    }
}
