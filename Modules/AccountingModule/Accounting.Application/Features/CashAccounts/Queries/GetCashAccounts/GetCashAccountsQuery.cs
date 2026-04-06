using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Queries.GetCashAccounts
{
    public class GetCashAccountsQuery : IRequest<List<CashAccountDto>>
    {
    }

    public class GetCashAccountsQueryHandler : IRequestHandler<GetCashAccountsQuery, List<CashAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCashAccountsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CashAccountDto>> Handle(GetCashAccountsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.CashAccounts.GetAllWithAccountAsync();

            return entities
                .Select(c => new CashAccountDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    AccountId = c.AccountId,
                    AccountCode = c.Account.Code,
                    AccountName = c.Account.NameAr
                })
                .ToList();
        }
    }
}
