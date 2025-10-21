using Inventory.Domain.Enums;

namespace Inventory.Application.Dtos.InventoryQuarantineDtos
{
    public class UpdateInventoryQuarantineDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime QuarantineDate { get; set; }
        public QuarantineStatus Status { get; set; }
        public string SourceReference { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
    }
}