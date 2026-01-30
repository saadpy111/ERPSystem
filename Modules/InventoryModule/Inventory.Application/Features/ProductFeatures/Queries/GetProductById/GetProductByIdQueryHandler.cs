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
        private readonly SharedKernel.Core.Files.IFileUrlResolver _urlResolver;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork , IproductRepo productRepo, SharedKernel.Core.Files.IFileUrlResolver urlResolver)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
            _urlResolver = urlResolver;
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
            var dto = product.ToDto(_urlResolver);

            if (attachments != null && attachments.Any())
            {
                dto.Attachments = attachments.ToList().ToDtoList(_urlResolver);
            }
            else
            {
                dto.Attachments = new List<AttachmentDto>();
            }

            return new GetProductByIdQueryResponse { Product = dto };
        }
    }
}