using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Dtos.CategoryDtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = request.categoryDto.ToEntity();

                if (request.categoryDto.Image != null)
                {
                    var path = await _fileService.SaveFileAsync(request.categoryDto.Image, "productcategories");
                    category.ImagePath = path.Replace("\\", "/");
                }

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
