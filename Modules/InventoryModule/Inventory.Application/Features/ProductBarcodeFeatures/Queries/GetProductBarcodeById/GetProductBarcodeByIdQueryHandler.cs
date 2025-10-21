using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetProductBarcodeById
{
    public class GetProductBarcodeByIdQueryHandler : IRequestHandler<GetProductBarcodeByIdQueryRequest, GetProductBarcodeByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductBarcodeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductBarcodeByIdQueryResponse> Handle(GetProductBarcodeByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repositories<ProductBarcode>().GetById(request.Id);
            if (entity == null)
                return new GetProductBarcodeByIdQueryResponse();

            return new GetProductBarcodeByIdQueryResponse
            {
                ProductBarcode = new GetProductBarcodeDto
                {
                    Id = entity.Id,
                    BarcodeValue = entity.BarcodeValue,
                    Type = entity.Type,
                    ProductId = entity.ProductId,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            };
        }
    }
}