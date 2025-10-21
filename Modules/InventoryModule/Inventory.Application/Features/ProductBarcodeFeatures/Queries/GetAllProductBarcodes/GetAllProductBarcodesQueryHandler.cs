using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetAllProductBarcodes
{
    public class GetAllProductBarcodesQueryHandler : IRequestHandler<GetAllProductBarcodesQueryRequest, GetAllProductBarcodesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductBarcodesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllProductBarcodesQueryResponse> Handle(GetAllProductBarcodesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductBarcode, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.BarcodeValue))
                filter = pb => pb.BarcodeValue.Contains(request.BarcodeValue);

            var barcodes = await _unitOfWork.Repositories<ProductBarcode>().GetAll(filter);

            var dtos = barcodes.Select(pb => new GetProductBarcodeDto
            {
                Id = pb.Id,
                BarcodeValue = pb.BarcodeValue,
                Type = pb.Type,
                ProductId = pb.ProductId,
                CreatedAt = pb.CreatedAt,
                UpdatedAt = pb.UpdatedAt
            }).ToList();

            return new GetAllProductBarcodesQueryResponse { ProductBarcodes = dtos };
        }
    }
}