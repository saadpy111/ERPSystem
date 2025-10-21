using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.DeleteCategory
{
    public class DeleteCategoryCommandRequest :IRequest<DeleteCategoryCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
