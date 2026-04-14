using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Accounting.Application.Common.Models;
using Accounting.Domain.Enums;

namespace Accounting.Application.Features.Partners.Queries.GetPartnersList
{
    public class PartnerDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public PartnerType Type { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetPartnersListQuery : IRequest<Result<List<PartnerDto>>>
    {
        public PartnerType? Type { get; set; }
    }

    public class GetPartnersListQueryHandler : IRequestHandler<GetPartnersListQuery, Result<List<PartnerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPartnersListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PartnerDto>>> Handle(GetPartnersListQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Partners.Query();

            if (request.Type.HasValue)
            {
                query = query.Where(p => p.Type == request.Type.Value);
            }

            var partners = await query
                .Select(p => new PartnerDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.NameAr,
                    Type = p.Type,
                    IsActive = p.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<PartnerDto>>.Ok(partners);
        }
    }
}
