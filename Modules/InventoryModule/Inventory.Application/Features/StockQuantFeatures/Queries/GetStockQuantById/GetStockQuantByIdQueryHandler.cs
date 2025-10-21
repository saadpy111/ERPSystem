using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockQuantDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetStockQuantById
{
    public class GetStockQuantByIdQueryHandler : IRequestHandler<GetStockQuantByIdQueryRequest, GetStockQuantByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockQuantByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetStockQuantByIdQueryResponse> Handle(GetStockQuantByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<StockQuant>().GetById(request.Id);
            if (entity == null)
                return new GetStockQuantByIdQueryResponse();

            return new GetStockQuantByIdQueryResponse
            {
                StockQuant = new GetStockQuantDto
                {
                    Id = entity.Id,
                    ProductId = entity.ProductId,
                    LocationId = entity.LocationId,
                    Quantity = entity.Quantity,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            };
        }
    }
}