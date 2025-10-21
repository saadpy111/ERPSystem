using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.InventoryQuarantineDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.InventoryQuarantineFeatures.Queries.GetInventoryQuarantineById
{
    public class GetInventoryQuarantineByIdQueryHandler : IRequestHandler<GetInventoryQuarantineByIdQueryRequest, GetInventoryQuarantineByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInventoryQuarantineByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetInventoryQuarantineByIdQueryResponse> Handle(GetInventoryQuarantineByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<InventoryQuarantine>().GetById(request.Id);
            if (entity == null)
                return new GetInventoryQuarantineByIdQueryResponse();

            return new GetInventoryQuarantineByIdQueryResponse
            {
                InventoryQuarantine = new GetInventoryQuarantineDto
                {
                    Id = entity.Id,
                    Quantity = entity.Quantity,
                    QuarantineDate = entity.QuarantineDate,
                    Status = entity.Status,
                    SourceReference = entity.SourceReference,
                    ProductId = entity.ProductId,
                    LocationId = entity.LocationId,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            };
        }
    }
}