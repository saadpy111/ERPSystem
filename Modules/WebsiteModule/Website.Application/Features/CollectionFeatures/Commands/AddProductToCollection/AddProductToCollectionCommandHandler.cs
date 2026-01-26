using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Commands.AddProductToCollection
{
    public class AddProductToCollectionCommandHandler : IRequestHandler<AddProductToCollectionCommandRequest, AddProductToCollectionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public AddProductToCollectionCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<AddProductToCollectionCommandResponse> Handle(AddProductToCollectionCommandRequest request, CancellationToken cancellationToken)
        {
            var collectionRepo = _unitOfWork.Repository<ProductCollection>();
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var itemRepo = _unitOfWork.Repository<ProductCollectionItem>();

            // Validate collection exists
            var collection = await collectionRepo.GetByIdAsync(request.CollectionId);
            if (collection == null)
            {
                return new AddProductToCollectionCommandResponse
                {
                    Success = false,
                    Message = "Collection not found."
                };
            }

            // Validate product exists
            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new AddProductToCollectionCommandResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            // Check if product already in collection
            var existing = await itemRepo.GetFirstAsync(i => i.CollectionId == request.CollectionId && i.ProductId == request.ProductId);
            if (existing != null)
            {
                return new AddProductToCollectionCommandResponse
                {
                    Success = false,
                    Message = "Product is already in this collection."
                };
            }

            // Add product to collection
            var item = new ProductCollectionItem
            {
                CollectionId = request.CollectionId,
                ProductId = request.ProductId,
                DisplayOrder = request.DisplayOrder,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await itemRepo.AddAsync(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddProductToCollectionCommandResponse { Success = true };
        }
    }
}
