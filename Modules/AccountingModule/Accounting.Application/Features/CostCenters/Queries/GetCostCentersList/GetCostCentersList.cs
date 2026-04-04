using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.CostCenters.Queries.GetCostCentersList
{
    public class CostCenterDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
    }

    public class GetCostCentersListQuery : IRequest<List<CostCenterDto>>
    {
    }

    public class GetCostCentersListQueryHandler : IRequestHandler<GetCostCentersListQuery, List<CostCenterDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetCostCentersListQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CostCenterDto>> Handle(GetCostCentersListQuery request, CancellationToken cancellationToken)
        {
            return await _context.CostCenters
                .AsNoTracking()
                .Select(c => new CostCenterDto { Id = c.Id, NameAr = c.Name })
                .ToListAsync(cancellationToken);
        }
    }
}
