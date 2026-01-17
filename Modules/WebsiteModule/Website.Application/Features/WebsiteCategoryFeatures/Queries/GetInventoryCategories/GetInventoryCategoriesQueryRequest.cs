using MediatR;

namespace Website.Application.Features.WebsiteCategoryFeatures.Queries.GetInventoryCategories
{
    public class GetInventoryCategoriesQueryRequest : IRequest<GetInventoryCategoriesQueryResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // Name, CreatedAt
        public string? SortDirection { get; set; } // Asc, Desc
        public Guid? ParentCategoryId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
