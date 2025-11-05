using Inventory.Application.Dtos.AttachmentDtos;
using System.Collections.Generic;

namespace Inventory.Application.Dtos.WarehouseDtos
{
    public class UpdateWarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? LocationDetails { get; set; } = "";
        public string? WarehouseCode { get; set; }
        public string? ResponsibleEmployee { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string? WarehouseType { get; set; }
        public string? FinancialAccountCode { get; set; }
        public decimal? PercentageUtilized { get; set; }
        public int? TotalStorageCapacity { get; set; }
        public string? InventoryPolicy { get; set; }
        public string? Government { get; set; }
        public bool EditAttachments { get; set; }
        public List<CreateAttachmentDto>? Attachments { get; set; }
    }
}