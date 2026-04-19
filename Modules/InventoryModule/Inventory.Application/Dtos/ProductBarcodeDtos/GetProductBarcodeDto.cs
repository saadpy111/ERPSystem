namespace Inventory.Application.Dtos.ProductBarcodeDtos
{
    public class GetProductBarcodeDto
    {
        public Guid Id { get; set; }
        public string BarcodeValue { get; set; }
        public string Type { get; set; }
        public bool IsWeighted { get; set; }
        public string? Prefix { get; set; }
        public Guid ProductId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}