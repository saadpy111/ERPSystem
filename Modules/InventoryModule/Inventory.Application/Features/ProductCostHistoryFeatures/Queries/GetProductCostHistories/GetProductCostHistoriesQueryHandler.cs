using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductCostHistoryDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistories
{
    public class GetProductCostHistoriesQueryHandler 
        : IRequestHandler<GetProductCostHistoriesQueryRequest, GetProductCostHistoriesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductCostHistoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductCostHistoriesQueryResponse> Handle(
            GetProductCostHistoriesQueryRequest request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<ProductCostHistory, bool>>? filter = null;

            if (request.ProductId.HasValue && request.FromDate.HasValue && request.ToDate.HasValue)
            {
                filter = h => h.ProductId == request.ProductId.Value && 
                             h.CreatedAt >= request.FromDate.Value &&
                             h.CreatedAt <= request.ToDate.Value;
            }
            else if (request.ProductId.HasValue)
            {
                filter = h => h.ProductId == request.ProductId.Value;
            }
            else if (request.FromDate.HasValue && request.ToDate.HasValue)
            {
                filter = h => h.CreatedAt >= request.FromDate.Value &&
                             h.CreatedAt <= request.ToDate.Value;
            }

            var histories = await _unitOfWork.Repositories<ProductCostHistory>().GetAll(filter);

            var dtos = histories.Select(h => new GetProductCostHistoryDto
            {
                Id = h.Id,
                OldCost = h.OldCost,
                NewCost = h.NewCost,
                ProductId = h.ProductId,
                CreatedAt = h.CreatedAt
            }).ToList();

            return new GetProductCostHistoriesQueryResponse { CostHistories = dtos };
        }
    }
}