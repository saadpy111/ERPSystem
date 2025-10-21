using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductCostHistoryDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductCostHistoryFeatures.Queries.GetProductCostHistoryById
{
    public class GetProductCostHistoryByIdQueryHandler 
        : IRequestHandler<GetProductCostHistoryByIdQueryRequest, GetProductCostHistoryByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductCostHistoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductCostHistoryByIdQueryResponse> Handle(
            GetProductCostHistoryByIdQueryRequest request, 
            CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<ProductCostHistory>()
                    .GetById(request.Id);
            if (entity == null)
                return new GetProductCostHistoryByIdQueryResponse();

            return new GetProductCostHistoryByIdQueryResponse
            {
                ProductCostHistory = new GetProductCostHistoryDto
                {
                    Id = entity.Id,
                    OldCost = entity.OldCost,
                    NewCost = entity.NewCost,
                    ProductId = entity.ProductId,
                    CreatedAt = entity.CreatedAt
                }
            };
        }
    }
}