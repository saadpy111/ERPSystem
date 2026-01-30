using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.Pagination;
using Website.Domain.Entities;
using SharedKernel.Multitenancy;
using System.Linq.Expressions;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCollections
{
    public class GetStorefrontCollectionsQueryHandler : IRequestHandler<GetStorefrontCollectionsQueryRequest, GetStorefrontCollectionsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public GetStorefrontCollectionsQueryHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
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
            
            // Map manually because it's paged
            var dtos = collections.Items.Select(c => new StorefrontCollectionDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = c.ImageUrl
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
