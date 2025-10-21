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

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
              _unitOfWork = unitOfWork;
        }
        public async Task<GetCategoryByIdQueryResponse> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repositories<ProductCategory>()
                                         .GetById(request.CategoryId 
                                           , pc =>pc.ChildCategories);

            var dto = category.ToDto();
            return new GetCategoryByIdQueryResponse()
            {
                CategoryDto = dto
            };


        }
    }
}
