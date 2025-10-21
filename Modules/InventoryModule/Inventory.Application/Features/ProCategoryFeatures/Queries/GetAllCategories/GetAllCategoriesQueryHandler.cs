using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
namespace Inventory.Application.Features.ProCategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQueryRequest, GetAllCategoriesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
               _unitOfWork = unitOfWork;
        }
        public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
           Expression<Func<ProductCategory, bool>> filter = PC => PC.ParentId == null; // get first layer with first children layer
            var categories = await _unitOfWork.Repositories<ProductCategory>()
                                  .GetAll(filter , c=>c.ChildCategories);

            var dtos = categories.Select(c => c.ToDto()).ToList();

            return new GetAllCategoriesQueryResponse()
            {
                Categories = dtos
            };
        }
    }
}
