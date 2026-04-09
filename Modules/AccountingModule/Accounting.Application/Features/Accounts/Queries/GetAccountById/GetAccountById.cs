using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Accounting.Application.Common.Exceptions;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Accounts.Queries.GetAccountById
{
    public class AccountDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public int AccountType { get; set; }
    }

    public class GetAccountByIdQuery : IRequest<Result<AccountDetailDto>>
    {
        public int Id { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountDetailDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetAccountByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AccountDetailDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (account == null)
            {
                throw new BusinessException("Account not found");
            }

            return new AccountDetailDto
            {
                Id = account.Id,
                Code = account.Code,
                NameAr = account.NameAr,
                AccountType = (int)account.AccountType
            };
        }
    }
}
