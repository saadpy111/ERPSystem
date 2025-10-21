using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockAdjustmentDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.StockAdjustmentFeatures.Queries.GetStockAdjustmentById
{
    public class GetStockAdjustmentByIdQueryHandler : IRequestHandler<GetStockAdjustmentByIdQueryRequest, GetStockAdjustmentByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockAdjustmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetStockAdjustmentByIdQueryResponse> Handle(GetStockAdjustmentByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<StockAdjustment>().GetById(request.Id);
            if (entity == null)
                return new GetStockAdjustmentByIdQueryResponse();

            return new GetStockAdjustmentByIdQueryResponse
            {
                StockAdjustment = new GetStockAdjustmentDto
                {
                    Id = entity.Id,
                    ExpectedQuantity = entity.ExpectedQuantity,
                    ActualQuantity = entity.ActualQuantity,
                    Date = entity.Date,
                    UserId = entity.UserId,
                    WarehouseId = entity.WarehouseId,
                    ProductId = entity.ProductId,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            };
        }
    }
}