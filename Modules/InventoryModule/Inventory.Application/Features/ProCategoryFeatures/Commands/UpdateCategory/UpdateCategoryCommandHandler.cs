using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _unitOfWork.Repositories<ProductCategory>().GetById(request.UpdateCategoryDto.Id);

                if (category == null)
                    return new UpdateCategoryCommandResponse()
                    {
                        Success = false,
                        Message = "الصنف غير موجود"
                    };
                var dto = request.UpdateCategoryDto;

                if (dto.Name != null)
                    category.Name = dto.Name;

                category.ParentId = dto.ParentId;
                _unitOfWork.Repositories<ProductCategory>().Update(category);
                await _unitOfWork.CompleteAsync();
                return new UpdateCategoryCommandResponse()
                {
                    Success = true,
                    Message = "تم بنجاح"
                };
            }
            catch
            {
                return new UpdateCategoryCommandResponse()
                {
                    Success = false,
                    Message = "حدث خطأ ما"
                };
            }
        
        }
    }
}
