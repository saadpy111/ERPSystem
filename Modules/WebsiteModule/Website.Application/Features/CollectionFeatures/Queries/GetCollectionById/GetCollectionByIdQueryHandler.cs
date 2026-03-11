using MediatR;
using SharedKernel.Core.Files;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Queries.GetCollectionById
{
    public class GetCollectionByIdQueryHandler : IRequestHandler<GetCollectionByIdQueryRequest, GetCollectionByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUrlResolver _urlResolver;

        public GetCollectionByIdQueryHandler(IUnitOfWork unitOfWork, IFileUrlResolver urlResolver)
        {
            _unitOfWork = unitOfWork;
            _urlResolver = urlResolver;
        }

        public async Task<GetCollectionByIdQueryResponse> Handle(GetCollectionByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(request.Id, c => c.Items);

            if (collection == null)
            {
                return new GetCollectionByIdQueryResponse { Collection = null };
            }

            var dto = new CollectionDetailDto
            {
                Id = collection.Id,
                Name = collection.Name,
                Slug = collection.Slug,
                Description = collection.Description,
                ImageUrl = _urlResolver.Resolve(collection.ImageUrl),
                IsActive = collection.IsActive,
                DisplayOrder = collection.DisplayOrder,
                Products = collection.Items.Select(i => new CollectionProductDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.NameSnapshot ?? string.Empty,
                    DisplayOrder = i.DisplayOrder
                }).ToList()
            };

            return new GetCollectionByIdQueryResponse { Collection = dto };
        }
    }
}
