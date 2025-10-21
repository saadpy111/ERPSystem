using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = request.categoryDto.ToEntity();

                await _unitOfWork.Repositories<ProductCategory>().Add(category);
                await _unitOfWork.CompleteAsync();
                return new CreateCategoryCommandResponse()
                {
                    Message = "تم بنجاح",
                    Success = true

                };
            }
            catch(Exception ex)
            {
                return new CreateCategoryCommandResponse()
                {
                    //
                    Message = "حدث خطأ ما",
                    Success = false


                };
            }
        }
    }
}
