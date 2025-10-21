using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.StockQuantDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.StockQuantFeatures.Queries.GetAllStockQuants
{
    public class GetAllStockQuantsQueryHandler : IRequestHandler<GetAllStockQuantsQueryRequest, GetAllStockQuantsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllStockQuantsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllStockQuantsQueryResponse> Handle(GetAllStockQuantsQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<StockQuant, bool>>? filter = null;
            if (request.ProductId.HasValue && request.LocationId.HasValue)
                filter = q => q.ProductId == request.ProductId.Value && q.LocationId == request.LocationId.Value;
            else if (request.ProductId.HasValue)
                filter = q => q.ProductId == request.ProductId.Value;
            else if (request.LocationId.HasValue)
                filter = q => q.LocationId == request.LocationId.Value;

            var quants = await _unitOfWork.Repositories<StockQuant>().GetAll(filter);

            var dtos = quants.Select(q => new GetStockQuantDto
            {
                Id = q.Id,
                ProductId = q.ProductId,
                LocationId = q.LocationId,
                Quantity = q.Quantity,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt
            }).ToList();

            return new GetAllStockQuantsQueryResponse { StockQuants = dtos };
        }
    }
}