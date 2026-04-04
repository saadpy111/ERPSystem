using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Partners.Queries.GetPartnerById
{
    public class PartnerDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetPartnerByIdQuery : IRequest<PartnerDetailDto>
    {
        public int Id { get; set; }
    }

    public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, PartnerDetailDto>
    {
        private readonly IAccountingDbContext _context;

        public GetPartnerByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<PartnerDetailDto> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
        {
            var partner = await _context.Partners
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (partner == null) throw new BusinessException("Not found");

            return new PartnerDetailDto { Id = partner.Id, Name = partner.NameAr };
        }
    }
}
