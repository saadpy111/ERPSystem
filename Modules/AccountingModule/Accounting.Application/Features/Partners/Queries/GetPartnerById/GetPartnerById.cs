using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Queries.GetPartnerById
{
    public class PartnerDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetPartnerByIdQuery : IRequest<Result<PartnerDetailDto>>
    {
        public int Id { get; set; }
    }

    public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, Result<PartnerDetailDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetPartnerByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PartnerDetailDto>> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
        {
            var partner = await _context.Partners
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (partner == null) throw new BusinessException("Not found");

            return new PartnerDetailDto { Id = partner.Id, Name = partner.NameAr };
        }
    }
}
