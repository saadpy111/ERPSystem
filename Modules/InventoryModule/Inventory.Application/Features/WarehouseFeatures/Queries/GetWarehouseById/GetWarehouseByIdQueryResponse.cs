using Inventory.Application.Dtos.WarehouseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.WarehouseFeatures.Queries.GetWarehouseById
{
    public class GetWarehouseByIdQueryResponse
    {
        public GetWarehouseDto?  WarehouseDto { get; set; }
    }
}
