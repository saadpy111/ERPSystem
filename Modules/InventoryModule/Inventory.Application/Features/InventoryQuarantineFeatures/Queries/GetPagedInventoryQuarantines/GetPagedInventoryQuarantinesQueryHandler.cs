using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.InventoryQuarantineDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetPagedInventoryQuarantines
{
    public class GetPagedInventoryQuarantinesQueryHandler : IRequestHandler<GetPagedInventoryQuarantinesQueryRequest, GetPagedInventoryQuarantinesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedInventoryQuarantinesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedInventoryQuarantinesQueryResponse> Handle(GetPagedInventoryQuarantinesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<InventoryQuarantine, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = q => q.ProductId.ToString().Contains(request.Search) ||
                              q.LocationId.ToString().Contains(request.Search)
                                ;

            var pagedResult = await _unitOfWork.Repositories<InventoryQuarantine>()
                .Search(filter, request.Page, request.PageSize);

            var dtoResult = new PagedResult<GetInventoryQuarantineDto>
            {
                Items = pagedResult.Items.Select(q => new GetInventoryQuarantineDto
                {
                    Id = q.Id,
                    Quantity = q.Quantity,
                    QuarantineDate = q.QuarantineDate,
                    Status = q.Status,
                    SourceReference = q.SourceReference,
                    ProductId = q.ProductId,
                    LocationId = q.LocationId,
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedInventoryQuarantinesQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}