using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CostCenters.Queries.GetCostCenterById
{
    public class CostCenterDetailsDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }
        public int? Level { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetCostCenterByIdQuery : IRequest<Result<CostCenterDetailsDto>>
    {
        public int Id { get; set; }
    }

    public class GetCostCenterByIdQueryHandler : IRequestHandler<GetCostCenterByIdQuery, Result<CostCenterDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCostCenterByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CostCenterDetailsDto>> Handle(GetCostCenterByIdQuery request, CancellationToken cancellationToken)
        {
            var cc = await _unitOfWork.CostCenters.GetByIdAsync(request.Id);
            if (cc == null) return Result<CostCenterDetailsDto>.Failure("Cost Center not found.");

            var dto = new CostCenterDetailsDto
            {
                Id = cc.Id,
                Code = cc.Code,
                NameAr = cc.NameAr,
                NameEn = cc.NameEn,
                ParentId = cc.ParentId,
                Level = cc.Level,
                IsActive = cc.IsActive
            };

            if (cc.ParentId.HasValue)
            {
                var parent = await _unitOfWork.CostCenters.GetByIdAsync(cc.ParentId.Value);
                dto.ParentName = parent?.NameAr;
            }

            return Result<CostCenterDetailsDto>.Ok(dto);
        }
    }
}
