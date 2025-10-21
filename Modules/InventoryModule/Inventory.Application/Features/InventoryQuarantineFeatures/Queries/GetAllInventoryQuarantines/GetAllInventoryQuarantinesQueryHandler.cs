using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.InventoryQuarantineDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetAllInventoryQuarantines
{
    public class GetAllInventoryQuarantinesQueryHandler : IRequestHandler<GetAllInventoryQuarantinesQueryRequest, GetAllInventoryQuarantinesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllInventoryQuarantinesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllInventoryQuarantinesQueryResponse> Handle(GetAllInventoryQuarantinesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<InventoryQuarantine, bool>>? filter = null;

            if (request.ProductId.HasValue && request.LocationId.HasValue && request.Status.HasValue)
                filter = q => q.ProductId == request.ProductId.Value && q.LocationId == request.LocationId.Value && q.Status == request.Status.Value;
            else if (request.ProductId.HasValue && request.LocationId.HasValue)
                filter = q => q.ProductId == request.ProductId.Value && q.LocationId == request.LocationId.Value;
            else if (request.ProductId.HasValue)
                filter = q => q.ProductId == request.ProductId.Value;
            else if (request.LocationId.HasValue)
                filter = q => q.LocationId == request.LocationId.Value;
            else if (request.Status.HasValue)
                filter = q => q.Status == request.Status.Value;

            var quants = await _unitOfWork.Repositories<InventoryQuarantine>().GetAll(filter);

            var dtos = quants.Select(q => new GetInventoryQuarantineDto
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
            }).ToList();

            return new GetAllInventoryQuarantinesQueryResponse
            {
                InventoryQuarantines = dtos
            };
        }
    }
}
