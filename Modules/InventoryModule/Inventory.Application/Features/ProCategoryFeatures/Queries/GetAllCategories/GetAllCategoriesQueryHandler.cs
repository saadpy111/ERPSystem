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
        private readonly SharedKernel.Core.Files.IFileUrlResolver _urlResolver;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, SharedKernel.Core.Files.IFileUrlResolver urlResolver)
        {
               _unitOfWork = unitOfWork;
               _urlResolver = urlResolver;
        }
        public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
           Expression<Func<ProductCategory, bool>> filter = PC => PC.ParentId == null; // get first layer with first children layer
            var categories = await _unitOfWork.Repositories<ProductCategory>()
                                  .GetAll(filter , c=>c.ChildCategories);

            var dtos = categories.Select(c => c.ToDto(_urlResolver)).ToList();

            return new GetAllCategoriesQueryResponse()
            {
                Categories = dtos
            };
        }
    }
}
