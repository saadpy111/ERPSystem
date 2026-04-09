using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Queries.GetPartnersList
{
    public class PartnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetPartnersListQuery : IRequest<Result<List<PartnerDto>>>
    {
    }

    public class GetPartnersListQueryHandler : IRequestHandler<GetPartnersListQuery, Result<List<PartnerDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetPartnersListQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<PartnerDto>>> Handle(GetPartnersListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Partners
                .AsNoTracking()
                .Select(p => new PartnerDto
                {
                    Id = p.Id,
                    Name = p.NameAr
                })
                .ToListAsync(cancellationToken);
        }
    }
}
