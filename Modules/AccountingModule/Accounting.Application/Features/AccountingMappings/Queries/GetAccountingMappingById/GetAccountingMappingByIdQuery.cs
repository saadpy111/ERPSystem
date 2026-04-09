using Accounting.Application.Features.AccountingMappings.DTOs;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.AccountingMappings.Queries.GetAccountingMappingById
{
    public class GetAccountingMappingByIdQuery : IRequest<AccountingMappingDto?>
    {
        public int Id { get; set; }
    }

    public class GetAccountingMappingByIdQueryHandler : IRequestHandler<GetAccountingMappingByIdQuery, AccountingMappingDto?>
    {
        private readonly IAccountingDbContext _context;

        public GetAccountingMappingByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<AccountingMappingDto?> Handle(GetAccountingMappingByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.AccountingMappings
                .AsNoTracking()
                .Include(m => m.Account)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (entity == null)
                return null;

            return new AccountingMappingDto
            {
                Id = entity.Id,
                SourceType = entity.SourceType,
                MappingKey = entity.MappingKey,
                AccountId = entity.AccountId,
                AccountName = entity.Account.NameAr,
                IsActive = entity.IsActive,
                Description = entity.Description
            };
        }
    }
}
