using Inventory.Application.Dtos.WarehouseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetAllWarehouses
{
    public class GetAllWarehousesQueryResponse
    {
        public List<GetWarehouseDto>   WarehouseDtos { get; set; }
    }
}
