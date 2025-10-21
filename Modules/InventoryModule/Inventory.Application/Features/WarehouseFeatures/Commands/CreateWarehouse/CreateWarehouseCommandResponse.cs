using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.WarehouseFeatures.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
