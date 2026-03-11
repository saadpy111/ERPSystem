using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using Website.Domain.Entities;
using SharedKernel.Multitenancy;
using System.Linq.Expressions;
using SharedKernel.Core.Files;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCollections
{
    public class GetStorefrontCollectionsQueryHandler : IRequestHandler<GetStorefrontCollectionsQueryRequest, GetStorefrontCollectionsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFileUrlResolver _fileUrlResolver;

        public GetStorefrontCollectionsQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider , IFileUrlResolver fileUrlResolver)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
            _fileUrlResolver = fileUrlResolver;
        }

        public async Task<GetStorefrontCollectionsQueryResponse> Handle(GetStorefrontCollectionsQueryRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();

            Expression<Func<ProductCollection, bool>> filter = c => c.IsActive;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                Expression<Func<ProductCollection, bool>> searchFilter = c => c.IsActive && (c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));
                filter = searchFilter;
            }

            var collections = await repo.SearchAsync(
                filter: filter,
                page: request.PageNumber,
                pageSize: request.PageSize,
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name));
            
            var dtos = collections.Items.Select(c => new StorefrontCollectionDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = _fileUrlResolver.Resolve(c.ImageUrl)
            }).ToList();

            var pagedResult = new PagedResult<StorefrontCollectionDto>
            {
                Items = dtos,
                TotalCount = collections.TotalCount,
                Page = collections.Page,
                PageSize = collections.PageSize
            };

            return new GetStorefrontCollectionsQueryResponse { Result = pagedResult };
        }
    }
}
