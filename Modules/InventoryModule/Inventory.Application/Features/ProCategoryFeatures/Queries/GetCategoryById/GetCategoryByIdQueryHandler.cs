using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Domain.Entities;
using MediatR;
using System.Security.Cryptography;


namespace Inventory.Application.Features.ProCategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQueryRequest, GetCategoryByIdQueryResponse>
    {
        private readonly IUnitOfWork   _unitOfWork;
        private readonly SharedKernel.Core.Files.IFileUrlResolver _urlResolver;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, SharedKernel.Core.Files.IFileUrlResolver urlResolver)
        {
              _unitOfWork = unitOfWork;
              _urlResolver = urlResolver;
        }
        public async Task<GetCategoryByIdQueryResponse> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repositories<ProductCategory>()
                                         .GetById(request.CategoryId 
                                           , pc =>pc.ChildCategories);
               if (category == null)
                return new GetCategoryByIdQueryResponse()
                {
                    CategoryDto = null
                };

            var dto = category.ToDto(_urlResolver);
            return new GetCategoryByIdQueryResponse()
            {
                CategoryDto = dto
            };


        }
    }
}
