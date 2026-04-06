using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Queries.GetCashAccountById
{
    public class GetCashAccountByIdQuery : IRequest<CashAccountDto?>
    {
        public int Id { get; set; }
    }

    public class GetCashAccountByIdQueryHandler : IRequestHandler<GetCashAccountByIdQuery, CashAccountDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCashAccountByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CashAccountDto?> Handle(GetCashAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CashAccounts.GetWithAccountAsync(request.Id);
            if (entity == null)
                return null;

            return new CashAccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountId = entity.AccountId,
                AccountCode = entity.Account.Code,
                AccountName = entity.Account.NameAr
            };
        }
    }
}
