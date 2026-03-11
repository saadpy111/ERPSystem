using MediatR;
using SharedKernel.Core.Files;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CollectionFeatures.Queries.GetAllCollections
{
    public class GetAllCollectionsQueryHandler : IRequestHandler<GetAllCollectionsQueryRequest, GetAllCollectionsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUrlResolver _urlResolver;

        public GetAllCollectionsQueryHandler(IUnitOfWork unitOfWork, IFileUrlResolver urlResolver)
        {
            _unitOfWork = unitOfWork;
            _urlResolver = urlResolver;
        }

        public async Task<GetAllCollectionsQueryResponse> Handle(GetAllCollectionsQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collections = await repo.GetAllAsync(
                filter: request.IsActive.HasValue ? c => c.IsActive == request.IsActive : null,
                includes: c => c.Items);

            var dtos = collections
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new CollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    ImageUrl = _urlResolver.Resolve(c.ImageUrl),
                    IsActive = c.IsActive,
                    DisplayOrder = c.DisplayOrder,
                    ProductCount = c.Items.Count
                }).ToList();

            return new GetAllCollectionsQueryResponse { Collections = dtos };
        }
    }
}
