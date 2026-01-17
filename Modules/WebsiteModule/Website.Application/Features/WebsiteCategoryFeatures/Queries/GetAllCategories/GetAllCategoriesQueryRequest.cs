using MediatR;

namespace Website.Application.Features.WebsiteCategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryRequest : IRequest<GetAllCategoriesQueryResponse>
    {
        public bool? IsActive { get; set; }
        public bool IncludeTree { get; set; } = false;
    }
}
