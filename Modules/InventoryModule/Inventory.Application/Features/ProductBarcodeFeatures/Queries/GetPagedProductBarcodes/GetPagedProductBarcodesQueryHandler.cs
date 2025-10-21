using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductBarcodeDtos;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Inventory.Application.Features.ProductBarcodeFeatures.Queries.GetPagedProductBarcodes
{
    public class GetPagedProductBarcodesQueryHandler : IRequestHandler<GetPagedProductBarcodesQueryRequest, GetPagedProductBarcodesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedProductBarcodesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetPagedProductBarcodesQueryResponse> Handle(GetPagedProductBarcodesQueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductBarcode, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = b => b.BarcodeValue.Contains(request.Search);

            var pagedResult = await _unitOfWork.Repositories<ProductBarcode>()
                .Search(filter, request.Page, request.PageSize);

            // Map to DTOs
            var dtoResult = new PagedResult<GetProductBarcodeDto>
            {
                Items = pagedResult.Items.Select(pb => new GetProductBarcodeDto
                {
                    Id = pb.Id,
                    BarcodeValue = pb.BarcodeValue,
                    Type = pb.Type,
                    ProductId = pb.ProductId,
                    CreatedAt = pb.CreatedAt,
                    UpdatedAt = pb.UpdatedAt
                }),
                TotalCount = pagedResult.TotalCount
            };

            return new GetPagedProductBarcodesQueryResponse
            {
                Result = dtoResult
            };
        }
    }
}