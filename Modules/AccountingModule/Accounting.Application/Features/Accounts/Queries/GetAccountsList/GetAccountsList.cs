using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Accounts.Queries.GetAccountsList
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
    }

    public class GetAccountsListQuery : IRequest<List<AccountDto>>
    {
    }

    public class GetAccountsListQueryHandler : IRequestHandler<GetAccountsListQuery, List<AccountDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetAccountsListQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDto>> Handle(GetAccountsListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    NameAr = a.NameAr
                })
                .ToListAsync(cancellationToken);
        }
    }
}
