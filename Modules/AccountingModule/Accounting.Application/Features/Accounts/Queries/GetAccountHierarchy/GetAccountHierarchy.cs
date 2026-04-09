using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Accounts.Queries.GetAccountHierarchy
{
    public class AccountTreeNodeDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public int? ParentAccountId { get; set; }
        public List<AccountTreeNodeDto> Children { get; set; } = new();
    }

    public class GetAccountHierarchyQuery : IRequest<Result<List<AccountTreeNodeDto>>>
    {
    }

    public class GetAccountHierarchyQueryHandler : IRequestHandler<GetAccountHierarchyQuery, Result<List<AccountTreeNodeDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetAccountHierarchyQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AccountTreeNodeDto>>> Handle(GetAccountHierarchyQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _context.Accounts
                .AsNoTracking()
                .Select(a => new AccountTreeNodeDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    NameAr = a.NameAr,
                    ParentAccountId = a.ParentAccountId
                })
                .ToListAsync(cancellationToken);

            var dict = accounts.ToDictionary(a => a.Id);
            var roots = new List<AccountTreeNodeDto>();

            foreach (var account in accounts)
            {
                if (account.ParentAccountId.HasValue && dict.TryGetValue(account.ParentAccountId.Value, out var parent))
                {
                    parent.Children.Add(account);
                }
                else
                {
                    roots.Add(account);
                }
            }

            return roots;
        }
    }
}
