using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.ProductDtos;
using Inventory.Application.Dtos.AttachmentDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq;

namespace Inventory.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IproductRepo _productRepo;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork , IproductRepo productRepo)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await  _productRepo
                                  .GetProductDetailsById(request.Id);
            if (product == null) return new GetProductByIdQueryResponse();

            // Get Attachments related to this Product (Polymorphic)
            var attachments = await _unitOfWork.Repositories<Attachment>()
                .GetAll(a => a.EntityType == nameof(Product) && a.EntityId == product.Id);

            //  Map to DTO
            var dto = product.ToDto();

            if (attachments != null && attachments.Any())
            {
                dto.Attachments = attachments.Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType,
                    Description = a.Description,
                    UploadedAt = a.UploadedAt
                }).ToList();
            }

            else
            {
                dto.Attachments = new List<AttachmentDto>();
            }
            if (attachments.Any())
            {
                dto.Attachments = attachments.ToDtoList();
            }

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}