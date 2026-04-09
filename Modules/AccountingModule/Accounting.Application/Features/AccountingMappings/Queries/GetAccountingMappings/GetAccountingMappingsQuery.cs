using Accounting.Application.Features.AccountingMappings.DTOs;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.AccountingMappings.Queries.GetAccountingMappings
{
    public class GetAccountingMappingsQuery : IRequest<Result<List<AccountingMappingDto>>>
    {
        public SourceType? SourceType { get; set; }
    }

    public class GetAccountingMappingsQueryHandler : IRequestHandler<GetAccountingMappingsQuery, Result<List<AccountingMappingDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetAccountingMappingsQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AccountingMappingDto>>> Handle(GetAccountingMappingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.AccountingMappings
                .AsNoTracking()
                .Include(m => m.Account)
                .AsQueryable();

            if (request.SourceType.HasValue)
            {
                query = query.Where(m => m.SourceType == request.SourceType.Value);
            }

            return await query
                .OrderBy(m => m.SourceType)
                .ThenBy(m => m.MappingKey)
                .Select(m => new AccountingMappingDto
                {
                    Id = m.Id,
                    SourceType = m.SourceType,
                    MappingKey = m.MappingKey,
                    AccountId = m.AccountId,
                    AccountName = m.Account.NameAr,
                    IsActive = m.IsActive,
                    Description = m.Description
                })
                .ToListAsync(cancellationToken);
        }
    }
}
