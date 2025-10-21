using Inventory.Application.Dtos.CategoryDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.UpdateCategory
{
    public class UpdateCategoryCommandRequest :IRequest<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryDto  UpdateCategoryDto { get; set; }
    }
}
