using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.ProCategoryFeatures.Commands.DeleteCategory
{
    public class DeleteCategoryCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
